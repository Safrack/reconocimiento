import os
import base64
import pickle
import torch
import numpy as np
from PIL import Image
from flask import Flask, request, jsonify
from flask_cors import CORS
from facenet_pytorch import MTCNN, InceptionResnetV1
import psycopg2
import io 

# --- Inicialización ---
app = Flask(__name__)
CORS(app)
device = 'cuda' if torch.cuda.is_available() else 'cpu'

# --- Cargar embeddings ---
with open('embeddings.pkl', 'rb') as f:
    known_faces = pickle.load(f)

# --- Inicializar modelos ---
mtcnn = MTCNN(image_size=160, margin=20, keep_all=False, device=device)
resnet = InceptionResnetV1(pretrained='vggface2').eval().to(device)

# --- Función para base de datos ---
def get_pabellon_por_ci(ci):
    try:
        conn = psycopg2.connect(
            dbname="proyecto_carcel",
            user="postgres",
            password="admin123",
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
        print("⚠️ Error de BD:", e)
        return "Error de BD"

# --- Ruta principal ---
@app.route('/')
def home():
    return "Servidor de reconocimiento facial funcionando!"

# --- Ruta de reconocimiento ---
@app.route('/reconocer', methods=['POST'])
def reconocer():
    try:
        data = request.get_json()
        imagen_base64 = data.get('imagen')

        if not imagen_base64:
            return jsonify({'autorizado': False, 'error': 'No se recibió imagen'})

        # --- Decodificar imagen ---
        content = imagen_base64.split(',')[1]
        img_bytes = base64.b64decode(content)
        img = Image.open(io.BytesIO(img_bytes)).convert('RGB')

        # --- Detectar y codificar rostro ---
        face_tensor = mtcnn(img)
        if face_tensor is None:
            return jsonify({'autorizado': False, 'error': 'No se detectó rostro'})

        face_embedding = resnet(face_tensor.unsqueeze(0).to(device))

        # --- Comparar con conocidos ---
        min_dist = float('inf')
        identity = None

        for known in known_faces:
            dist = (face_embedding - known['embedding'].to(device)).norm().item()
            if dist < min_dist:
                min_dist = dist
                identity = known['name']

        if min_dist < 0.9:  # <-- umbral de reconocimiento
            nombre, ci = identity.split('_', 1)
            pabellon = get_pabellon_por_ci(ci)
            return jsonify({
                'autorizado': True,
                'nombre': nombre,
                'ci': ci,
                'pabellon': pabellon
            })

        return jsonify({'autorizado': False, 'error': 'Rostro no reconocido'})

    except Exception as e:
        print("⚠️ Error interno:", e)
        return jsonify({'autorizado': False, 'error': str(e)})

# --- Ejecutar servidor ---
if __name__ == '__main__':
    app.run(debug=True, port=5000)
