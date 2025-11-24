from PIL import Image
import os

known_faces_dir = 'known_faces'
backup_dir = 'known_faces_backup'

# 1. Crea una carpeta de respaldo
if not os.path.exists(backup_dir):
    os.makedirs(backup_dir)

print(f"Buscando imágenes en {known_faces_dir} para estandarizar...")

# 2. Procesa y estandariza cada archivo
for filename in os.listdir(known_faces_dir):
    if filename.endswith((".jpg", ".png")):
        try:
            filepath = os.path.join(known_faces_dir, filename)
            
            # Mueve el original al backup antes de reescribir
            os.rename(filepath, os.path.join(backup_dir, filename))
            
            # Carga la imagen desde el backup
            img = Image.open(os.path.join(backup_dir, filename))
            
            # Clave: Convierte el modo a RGB (8-bit)
            # Esto corrige los problemas de CMYK y otros formatos no compatibles
            if img.mode != 'RGB':
                img = img.convert('RGB')
            
            # 3. Guarda la imagen estandarizada de vuelta en la carpeta original
            img.save(filepath, quality=95)
            print(f"Éxito: {filename} estandarizada a RGB.")
            
        except Exception as e:
            print(f"ERROR: No se pudo procesar {filename}. Causa: {e}")
            
print("\n¡Proceso de estandarización completado!")