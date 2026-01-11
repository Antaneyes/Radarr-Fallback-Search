# Guia Técnica DEFINITIVA: Implementación de mHD en Sonarr

Esta guía contiene el código fuente **exacto y completo** utilizado en Radarr para el soporte de MicroHD (mHD). Sigue estos pasos para una portabilidad idéntica.

---

## 1. Modificadores de Calidad
**Archivo:** `src/NzbDrone.Core/Qualities/Modifier.cs`

Añade el valor al final del enum:
```csharp
public enum Modifier
{
    NONE = 0,
    REGIONAL,
    SCREENER,
    RAWHD,
    BRDISK,
    REMUX,
    MICROHD // ID interno para MicroHD
}
```

---

## 2. Catálogo Maestro de Calidades
**Archivo:** `src/NzbDrone.Core/Qualities/Quality.cs`

### A. Definiciones de Calidad (16 Variantes mHD)
Añade estas propiedades estáticas. Nota que los IDs (40-55) deben estar libres.

```csharp
// mHD Variants
public static Quality HDTV720pmHD    => new Quality(40, "HDTV-720p-mHD", QualitySource.TV, 720, Modifier.MICROHD);
public static Quality HDTV1080pmHD   => new Quality(41, "HDTV-1080p-mHD", QualitySource.TV, 1080, Modifier.MICROHD);
public static Quality HDTV2160pmHD   => new Quality(42, "HDTV-2160p-mHD", QualitySource.TV, 2160, Modifier.MICROHD);
public static Quality WEBDL480pmHD   => new Quality(43, "WEBDL-480p-mHD", QualitySource.WEBDL, 480, Modifier.MICROHD);
public static Quality WEBDL720pmHD   => new Quality(44, "WEBDL-720p-mHD", QualitySource.WEBDL, 720, Modifier.MICROHD);
public static Quality WEBDL1080pmHD  => new Quality(45, "WEBDL-1080p-mHD", QualitySource.WEBDL, 1080, Modifier.MICROHD);
public static Quality WEBDL2160pmHD  => new Quality(46, "WEBDL-2160p-mHD", QualitySource.WEBDL, 2160, Modifier.MICROHD);
public static Quality WEBRip480pmHD  => new Quality(47, "WEBRip-480p-mHD", QualitySource.WEBRIP, 480, Modifier.MICROHD);
public static Quality WEBRip720pmHD  => new Quality(48, "WEBRip-720p-mHD", QualitySource.WEBRIP, 720, Modifier.MICROHD);
public static Quality WEBRip1080pmHD => new Quality(49, "WEBRip-1080p-mHD", QualitySource.WEBRIP, 1080, Modifier.MICROHD);
public static Quality WEBRip2160pmHD => new Quality(50, "WEBRip-2160p-mHD", QualitySource.WEBRIP, 2160, Modifier.MICROHD);
public static Quality Bluray480pmHD  => new Quality(51, "Bluray-480p-mHD", QualitySource.BLURAY, 480, Modifier.MICROHD);
public static Quality Bluray576pmHD  => new Quality(52, "Bluray-576p-mHD", QualitySource.BLURAY, 576, Modifier.MICROHD);
public static Quality Bluray720pmHD  => new Quality(53, "Bluray-720p-mHD", QualitySource.BLURAY, 720, Modifier.MICROHD);
public static Quality Bluray1080pmHD => new Quality(54, "Bluray-1080p-mHD", QualitySource.BLURAY, 1080, Modifier.MICROHD);
public static Quality Bluray2160pmHD => new Quality(55, "Bluray-2160p-mHD", QualitySource.BLURAY, 2160, Modifier.MICROHD);
```

### B. Registro en la Lista `All`
Dentro de `static Quality()`, añade todas al final de la lista para su correcta indexación:
```csharp
All = new List<Quality>
{
    Unknown, WORKPRINT, CAM, TELESYNC, TELECINE, DVDSCR, REGIONAL,
    SDTV, DVD, DVDR,
    HDTV720p, HDTV1080p, HDTV2160p,
    WEBDL480p, WEBDL720p, WEBDL1080p, WEBDL2160p,
    WEBRip480p, WEBRip720p, WEBRip1080p, WEBRip2160p,
    Bluray480p, Bluray576p, Bluray720p, Bluray1080p, Bluray2160p,
    Remux1080p, Remux2160p, BRDISK, RAWHD,
    // mHD variants
    HDTV720pmHD, HDTV1080pmHD, HDTV2160pmHD,
    WEBDL480pmHD, WEBDL720pmHD, WEBDL1080pmHD, WEBDL2160pmHD,
    WEBRip480pmHD, WEBRip720pmHD, WEBRip1080pmHD, WEBRip2160pmHD,
    Bluray480pmHD, Bluray576pmHD, Bluray720pmHD, Bluray1080pmHD, Bluray2160pmHD
};
```

### C. Configuración Completa de Pesos (DefaultQualityDefinitions)
Aplica este bloque íntegro en `Quality.cs`. Los pesos garantizan que `Estándar > mHD`.

```csharp
DefaultQualityDefinitions = new HashSet<QualityDefinition>
{
    new QualityDefinition(Quality.Unknown)     { Weight = 1,  MinSize = 0, MaxSize = 100, PreferredSize = 95 },
    new QualityDefinition(Quality.WORKPRINT)   { Weight = 2,  MinSize = 0, MaxSize = 100, PreferredSize = 95 },
    new QualityDefinition(Quality.CAM)         { Weight = 3,  MinSize = 0, MaxSize = 100, PreferredSize = 95 },
    new QualityDefinition(Quality.TELESYNC)    { Weight = 4,  MinSize = 0, MaxSize = 100, PreferredSize = 95 },
    new QualityDefinition(Quality.TELECINE)    { Weight = 5,  MinSize = 1, MaxSize = 100, PreferredSize = 95 },
    new QualityDefinition(Quality.REGIONAL)    { Weight = 6,  MinSize = 0, MaxSize = 100, PreferredSize = 95 },
    new QualityDefinition(Quality.DVDSCR)      { Weight = 7,  MinSize = 0, MaxSize = 100, PreferredSize = 95 },
    new QualityDefinition(Quality.SDTV)        { Weight = 8,  MinSize = 0, MaxSize = 100, PreferredSize = 95 },
    new QualityDefinition(Quality.DVD)         { Weight = 9,  MinSize = 0, MaxSize = 100, PreferredSize = 95 },
    new QualityDefinition(Quality.DVDR)        { Weight = 10, MinSize = 0, MaxSize = 100, PreferredSize = 95 },

    // 480p/576p
    new QualityDefinition(Quality.WEBDL480pmHD)  { Weight = 11, GroupName = "WEB 480p mHD" },
    new QualityDefinition(Quality.WEBRip480pmHD) { Weight = 11, GroupName = "WEB 480p mHD" },
    new QualityDefinition(Quality.WEBDL480p)     { Weight = 12, GroupName = "WEB 480p" },
    new QualityDefinition(Quality.WEBRip480p)    { Weight = 12, GroupName = "WEB 480p" },
    new QualityDefinition(Quality.Bluray480pmHD) { Weight = 13 },
    new QualityDefinition(Quality.Bluray480p)    { Weight = 14 },
    new QualityDefinition(Quality.Bluray576pmHD) { Weight = 15 },
    new QualityDefinition(Quality.Bluray576p)    { Weight = 16 },

    // 720p
    new QualityDefinition(Quality.HDTV720pmHD)   { Weight = 17 },
    new QualityDefinition(Quality.HDTV720p)      { Weight = 18 },
    new QualityDefinition(Quality.WEBDL720pmHD)  { Weight = 19, GroupName = "WEB 720p mHD" },
    new QualityDefinition(Quality.WEBRip720pmHD) { Weight = 19, GroupName = "WEB 720p mHD" },
    new QualityDefinition(Quality.WEBDL720p)     { Weight = 20, GroupName = "WEB 720p" },
    new QualityDefinition(Quality.WEBRip720p)    { Weight = 20, GroupName = "WEB 720p" },
    new QualityDefinition(Quality.Bluray720pmHD) { Weight = 21 },
    new QualityDefinition(Quality.Bluray720p)    { Weight = 22 },

    // 1080p
    new QualityDefinition(Quality.HDTV1080pmHD)  { Weight = 23 },
    new QualityDefinition(Quality.HDTV1080p)     { Weight = 24 },
    new QualityDefinition(Quality.WEBDL1080pmHD) { Weight = 25, GroupName = "WEB 1080p mHD" },
    new QualityDefinition(Quality.WEBRip1080pmHD) { Weight = 25, GroupName = "WEB 1080p mHD" },
    new QualityDefinition(Quality.WEBDL1080p)    { Weight = 26, GroupName = "WEB 1080p" },
    new QualityDefinition(Quality.WEBRip1080p)   { Weight = 26, GroupName = "WEB 1080p" },
    new QualityDefinition(Quality.Bluray1080pmHD) { Weight = 27 },
    new QualityDefinition(Quality.Bluray1080p)   { Weight = 28 },
    new QualityDefinition(Quality.Remux1080p)    { Weight = 30 },

    // 2160p
    new QualityDefinition(Quality.HDTV2160pmHD)  { Weight = 31 },
    new QualityDefinition(Quality.HDTV2160p)     { Weight = 32 },
    new QualityDefinition(Quality.WEBDL2160pmHD) { Weight = 33, GroupName = "WEB 2160p mHD" },
    new QualityDefinition(Quality.WEBRip2160pmHD) { Weight = 33, GroupName = "WEB 2160p mHD" },
    new QualityDefinition(Quality.WEBDL2160p)    { Weight = 34, GroupName = "WEB 2160p" },
    new QualityDefinition(Quality.WEBRip2160p)   { Weight = 34, GroupName = "WEB 2160p" },
    new QualityDefinition(Quality.Bluray2160pmHD) { Weight = 35 },
    new QualityDefinition(Quality.Bluray2160p)   { Weight = 36 },
    new QualityDefinition(Quality.Remux2160p)    { Weight = 40 },

    new QualityDefinition(Quality.BRDISK)        { Weight = 45 },
    new QualityDefinition(Quality.RAWHD)         { Weight = 46 }
};
```

---

## 3. Motor de Parseo (Lógica de Detección)
**Archivo:** `src/NzbDrone.Core/Parser/QualityParser.cs`

### A. Expresiones Regulares
Actualiza las regex para capturar todos los tags de MicroHD y resoluciones prefijadas:

```csharp
// Soporte m-720, m-1080, muHD, m4k
private static readonly Regex ResolutionRegex = new (@"\b(?:(?<R360p>360p)|(?<R480p>480p|480i|640x480|848x480)|(?<R540p>540p)|(?<R576p>576p)|(?<R720p>720p|1280x720|960p|m-?720)|(?<R1080p>1080p|1920x1080|1440p|FHD|1080i|4kto1080p|m-?1080)|(?<R2160p>2160p|3840x2160|4k[-_. ](?:UHD|HEVC|BD|H\.?265)|(?:UHD|HEVC|BD|H\.?265)[-_. ]4k|m-?2160|m-?4k|muhd))\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

// Tags específicos para activar el modificador MICROHD
private static readonly Regex MHDRegex = new (@"\b(?<mhd>mHD|micro[-_. ]?HD|μHD|m-?720|m-?1080|m-?2160|muhd|m-?4k)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
```

### B. Remapeo en `ParseQualityName` (BLOQUE COMPLETO)
Este bloque intercepta la calidad detectada y la convierte a su variante mHD si hay match:

```csharp
if (mhdMatch && result.Quality != Quality.Unknown)
{
    if (result.Quality == Quality.HDTV720p)   result.Quality = Quality.HDTV720pmHD;
    if (result.Quality == Quality.HDTV1080p)  result.Quality = Quality.HDTV1080pmHD;
    if (result.Quality == Quality.HDTV2160p)  result.Quality = Quality.HDTV2160pmHD;
    
    if (result.Quality == Quality.WEBDL480p)  result.Quality = Quality.WEBDL480pmHD;
    if (result.Quality == Quality.WEBDL720p)  result.Quality = Quality.WEBDL720pmHD;
    if (result.Quality == Quality.WEBDL1080p) result.Quality = Quality.WEBDL1080pmHD;
    if (result.Quality == Quality.WEBDL2160p) result.Quality = Quality.WEBDL2160pmHD;
    
    if (result.Quality == Quality.WEBRip480p) result.Quality = Quality.WEBRip480pmHD;
    if (result.Quality == Quality.WEBRip720p) result.Quality = Quality.WEBRip720pmHD;
    if (result.Quality == Quality.WEBRip1080p) result.Quality = Quality.WEBRip1080pmHD;
    if (result.Quality == Quality.WEBRip2160p) result.Quality = Quality.WEBRip2160pmHD;
    
    if (result.Quality == Quality.Bluray480p) result.Quality = Quality.Bluray480pmHD;
    if (result.Quality == Quality.Bluray576p) result.Quality = Quality.Bluray576pmHD;
    if (result.Quality == Quality.Bluray720p) result.Quality = Quality.Bluray720pmHD;
    if (result.Quality == Quality.Bluray1080p) result.Quality = Quality.Bluray1080pmHD;
    if (result.Quality == Quality.Bluray2160p) result.Quality = Quality.Bluray2160pmHD;
    
    result.ModifierDetectionSource = QualityDetectionSource.Name;
}
```

---

## 4. Persistencia en Perfiles (Sincronización de Base de Datos)
**Archivo:** `src/NzbDrone.Core/Profiles/Qualities/QualityProfileService.cs`

Para que las calidades mHD aparezcan en la UI de perfiles existentes tras el arranque:

```csharp
public void Handle(ApplicationStartedEvent message)
{
    if (!All().Any()) { /* Setup inicial */ }
    else { SyncMissingQualities(); }
}

private void SyncMissingQualities()
{
    var profiles = All();
    var allQualityIds = Quality.All.Where(q => q.Id > 0).Select(q => q.Id).ToList();

    foreach (var profile in profiles)
    {
        var profileQualities = profile.Items.SelectMany(i => i.GetQualities()).Select(q => q.Id).ToHashSet();
        if (allQualityIds.Any(id => !profileQualities.Contains(id)))
        {
            var allowedIds = profile.Items.Where(i => i.Allowed).SelectMany(i => i.GetQualities()).Select(q => q.Id).ToHashSet();
            var tempProfile = GetDefaultProfile(profile.Name, null, Quality.All.Where(q => allowedIds.Contains(q.Id)).ToArray());
            profile.Items = tempProfile.Items;
            Update(profile);
        }
    }
}
```
