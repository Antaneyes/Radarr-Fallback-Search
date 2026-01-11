# Radarr Fallback Search

Bienvenido al repositorio oficial de **Radarr Fallback Search**. Este proyecto es una evolución personalizada de Radarr que introduce capacidades avanzadas de búsqueda con indexadores de respaldo (fallback).

## 🚀 Funcionalidades Principales

### 1. Indexadores de Fallback
Ahora puedes marcar cualquier indexador como "Fallback" en su configuración.
- **Búsqueda Automática:** Radarr primero consultará tus indexadores normales. Solo si no se encuentra ningún resultado válido (aprobado), se procederá a consultar los indexadores marcados como fallback de forma secuencial.
- **Reducción de Latencia:** Evita que indexadores lentos retrasen toda la búsqueda si ya tienes resultados válidos en tus fuentes principales.

### 2. Búsqueda Interactiva bajo demanda
En la pantalla de búsqueda interactiva de películas:
- Se añade un nuevo botón: **"Buscar en Fallback"**.
- Esto te permite realizar una búsqueda normal rápida y, solo si no encuentras lo que buscas, disparar la búsqueda en los indexadores de fallback con un solo clic.

## 🛠️ Instalación y Despliegue (Docker)

Esta versión incluye herramientas para un despliegue rápido sobre la imagen oficial de Radarr mediante el montaje selectivo de binarios:

1. **Requisitos:** Tener Docker y Docker Compose instalados.
2. **Lanzamiento:** Ejecuta el archivo `launch_docker.bat` o usa el comando:
   ```bash
   docker-compose up -d
   ```
3. **Acceso:** La interfaz estará disponible por defecto en `http://localhost:7878`.

## 📂 Estructura del Proyecto

Los cambios más importantes se encuentran en:
- `NzbDrone.Core/IndexerSearch/ReleaseSearchService.cs`: Lógica de despacho secuencial.
- `Radarr.Api.V3/Indexers/ReleaseController.cs`: Soporte para el parámetro `includeFallback`.
- `frontend/src/InteractiveSearch/InteractiveSearch.tsx`: Nueva UI para búsquedas manuales en fallback.

---
*Este es el repositorio principal del proyecto **Radarr Fallback Search**. Basado en el código original de Radarr.*
