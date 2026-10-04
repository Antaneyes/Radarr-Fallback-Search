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

### 3. Carpetas nuevas con títulos localizados
Al crear una película nueva, los tokens de carpeta basados en `{Movie Title}` usan el idioma configurado en **Settings > UI > Movie Info Language**. Esto permite que formatos como `{Movie Title} ({Release Year})` creen carpetas en español cuando Radarr ya tiene esa traducción disponible.

> [!NOTE]
> Este cambio solo afecta a carpetas nuevas. No renombra carpetas ya existentes ni cambia los nombres de archivo de las películas.

## 🛠️ Instalación y Despliegue (Docker)

El `Dockerfile` de este repo compila Radarr completo (backend y UI) y genera una imagen basada en la de LinuxServer, con el programa original sustituido entero por esta versión:

```bash
docker build -t local/radarr-fallback:<versión> .
```

Después basta con usar esa imagen en el compose (`image: local/radarr-fallback:<versión>`), sin montar binarios. El `docker-compose.yml` del repo es un ejemplo que la construye directamente (`docker compose up -d --build`, o `launch_docker.bat` en Windows). La interfaz estará en `http://localhost:7878`.

> [!IMPORTANT]
> `LSIO_TAG` en el `Dockerfile` debe ser la versión de upstream en la que se basa el fork (p. ej. `6.4.4.10685-ls318`). Actualízala cada vez que se fusione una versión nueva de Radarr.

## 📂 Estructura del Proyecto

Los cambios más importantes se encuentran en:
- `NzbDrone.Core/IndexerSearch/ReleaseSearchService.cs`: Lógica de despacho secuencial.
- `Radarr.Api.V3/Indexers/ReleaseController.cs`: Soporte para el parámetro `includeFallback`.
- `NzbDrone.Core/Organizer/FileNameBuilder.cs`: Uso del idioma configurado para los títulos de carpetas nuevas.
- `frontend/src/InteractiveSearch/InteractiveSearch.tsx`: Nueva UI para búsquedas manuales en fallback.
- `Dockerfile`: Compilación del backend y la UI y generación de la imagen sobre la de LinuxServer.

---
*Este es el repositorio principal del proyecto **Radarr Fallback Search**. Basado en el código original de Radarr.*
