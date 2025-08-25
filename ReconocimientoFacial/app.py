import face_recognition
import cv2
import os
import psycopg2
import numpy as np
from flask import Flask, request, jsonify
from flask_cors import CORS
import base64

# --- Cargar rostros conocidos una vez ---
known_faces_dir = 'known_faces'
known_encodings = []
known_names = []

for filename in os.listdir(known_faces_dir):
    if filename.endswith(".jpg") or filename.endswith(".png"):
        image = face_recognition.load_image_file(os.path.join(known_faces_dir, filename))
        encodings = face_recognition.face_encodings(image)
        if encodings:
            known_encodings.append(encodings[0])
            known_names.append(filename.split('.')[0])

# --- Base de datos solo cuando se necesita ---
def get_pabellon_por_ci(ci):
    try:
        conn = psycopg2.connect(
            dbname="proyecto_carcel",
            user="postgres",
            password="lu123",
            host="localhost",
            port="5432"
        )
        cursor = conn.cursor()
        cursor.execute("""
            SELECT p.nombre_pb 
            FROM visitantes v
            JOIN pabellones p ON v.id_pabellones = p.id_pabellones
            WHERE v.ci = %s
        """, (ci,))
        result = cursor.fetchone()
        cursor.close()
        conn.close()
        return result[0] if result else "No registrado"
    except Exception as e:
        return "Error de BD"

# --- Flask App ---
app = Flask(__name__)
CORS(app)

@app.route('/reconocer', methods=['POST'])
def reconocer():
    data = request.get_json()
    imagen_base64 = data.get('imagen')

    if not imagen_base64:
        return jsonify({'autorizado': False, 'error': 'No se recibió imagen'})

    try:
        content = imagen_base64.split(',')[1]
        img_bytes = base64.b64decode(content)
        np_arr = np.frombuffer(img_bytes, np.uint8)
        frame = cv2.imdecode(np_arr, cv2.IMREAD_COLOR)
        rgb_frame = frame[:, :, ::-1]

        face_locations = face_recognition.face_locations(rgb_frame)
        face_encodings = face_recognition.face_encodings(rgb_frame, face_locations)

        for face_encoding in face_encodings:
            matches = face_recognition.compare_faces(known_encodings, face_encoding)
            face_distances = face_recognition.face_distance(known_encodings, face_encoding)
            best_match_index = face_distances.argmin() if len(face_distances) > 0 else -1

            if best_match_index >= 0 and matches[best_match_index]:
                name_ci = known_names[best_match_index]
                if "_" in name_ci:
                    nombre, ci = name_ci.split("_", 1)
                    pabellon = get_pabellon_por_ci(ci)
                    return jsonify({
                        'autorizado': True,
                        'nombre': nombre,
                        'ci': ci,
                        'pabellon': pabellon
                    })

        return jsonify({'autorizado': False})
    except Exception as e:
        return jsonify({'autorizado': False, 'error': str(e)})

if __name__ == '__main__':
    app.run(debug=True, port=5000)
