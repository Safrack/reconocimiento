import os
import torch
from facenet_pytorch import MTCNN, InceptionResnetV1
from PIL import Image
import pickle

# --- Configuración ---
device = 'cuda' if torch.cuda.is_available() else 'cpu'
known_faces_dir = 'known_faces'
embeddings_file = 'embeddings.pkl'

# --- Inicializar detectores y modelo ---
mtcnn = MTCNN(image_size=160, margin=20, keep_all=False, device=device)
resnet = InceptionResnetV1(pretrained='vggface2').eval().to(device)

embeddings_data = []

# --- Leer imágenes y generar embeddings ---
for filename in os.listdir(known_faces_dir):
    if filename.lower().endswith(('.jpg', '.png')):
        path = os.path.join(known_faces_dir, filename)
        img = Image.open(path).convert('RGB')  # <-- asegurar 3 canales
        face_tensor = mtcnn(img)

        if face_tensor is not None:
            face_embedding = resnet(face_tensor.unsqueeze(0).to(device))
            embeddings_data.append({
                'name': filename.split('.')[0],
                'embedding': face_embedding.detach().cpu()
            })
            print(f"✅ Procesado: {filename}")
        else:
            print(f"⚠️ No se detectó rostro en: {filename}")

# --- Guardar embeddings ---
with open(embeddings_file, 'wb') as f:
    pickle.dump(embeddings_data, f)

print(f"✅ Embeddings guardados en '{embeddings_file}'")
