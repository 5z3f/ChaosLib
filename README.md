<img align="left" src="logo.png" width="256px">

# ChaosLib λ

    // ------------------------------------------------------------
    // ChaosLib - Last Chaos Library (Alpha, under construction)
    // Written in .NET 5 | C# 9.0
    // ------------------------------------------------------------
    // ChaosLib.D3D        3D Utilites
    // ChaosLib.MAP        Game Data Mapper
    // ------------------------------------------------------------


<br/>

## [ChaosLib.D3D](https://github.com/5z3f/ChaosLib/tree/main/ChaosLib.D3D)
### Currently Available
* __Binary Importer__
  * ``BM`` Last Chaos Mesh (Version 16 & 17)
  * ``BM`` Serious Engine 1.10 Mesh (Version 11 & 12)
  * ``BS`` Last Chaos Skeleton
  * ``BA`` Last Chaos Animation
  * ``BAE`` Last Chaos Animation Effect
  * ``TEX`` Last Chaos Texture
* __Binary Exporter__
  * ``BM`` Last Chaos Mesh (Version 17)
  * ``BM`` Serious Engine 1.10 Mesh (Version 12)
  * ``BS`` Last Chaos Skeleton
  * ``BA`` Last Chaos Animation
  * ``BAE`` Last Chaos Animation Effect
  * ``TEX`` Last Chaos Texture (Uncompressed)
* __ASCII Exporter__
  * ``AM`` Mesh 0.1
  * ``AS`` Skeleton 0.1
  * ``AA`` Animation 0.1
  * ``AAL`` Animset List
* __OBJ Exporter__
  * ``OBJ`` Mesh (with UV) (this format does not support weight maps which are needed for animations)
* __GLTF Exporter__
  * ``GLTF/GLB`` Mesh (without weight maps) (if .png textures exists in saving directory they will be packed into .glb file)

## [ChaosLib.MAP](https://github.com/5z3f/ChaosLib/tree/main/ChaosLib.Map)
### Currently Available
* __Binary Importer__
  * ``LOD`` MobAll (NPC)
  * ``LOD`` Action
  * ``LOD`` Title
  * ``LOD`` MonsterCombo
  * ``BIN`` LevelGuide
  * ``LOD`` ItemExchange
  * ``DTA`` Help (help1)
  * ``DTA`` Map
  * ``BIN`` ArmorPreview
  * ``WTR`` WorldTerrain (Version 19 & 20)
  * ``SAT`` ServerAttributeMap
  * ``SHT`` ServerHeightMap
* __Binary Exporter__
  * ``LOD`` MobAll (NPC)
  * ``LOD`` Action
  * ``LOD`` Title
  * ``LOD`` MonsterCombo
  * ``BIN`` LevelGuide
  * ``LOD`` ItemExchange
  * ``DTA`` Help (help1)
  * ``DTA`` Map
  * ``BIN`` ArmorPreview
* __Mapped with database__
  * ``LOD`` NPC
  * ``LOD`` Action
  * ``LOD`` Title
  * ``LOD`` MonsterCombo
  * ``LOD`` ItemExchange
  * ``DTA`` Help (help1)

## Known issues
* Rotation in animation is unstable, quaternion correction needed
* Skeleton should be child->parent sorted before exporting
  
## Showcase
### ``.BA >> .AA >> Milkshape 3D >> .FBX >> Cinema 4D``

```cs
ChaosAsset chaosAsset = new ChaosAsset();
var dataObject = chaosAsset.Import(AssetType.Animation, "besurel.ba", AssetDataType.Binary);
chaosAsset.Export(AssetType.Animation, dataObject.Animations[0], "attack01.aa", AssetDataType.ASCII);
```

![besurel_attack01.aa](example/img/besurel-attack01.gif)

### ``.BS >> .AS >> Milkshape 3D >> .FBX >> Cinema 4D``

```cs
ChaosAsset chaosAsset = new ChaosAsset();
var dataObject = chaosAsset.Import(AssetType.Skeleton, "besurel.bs", AssetDataType.Binary);
chaosAsset.Export(AssetType.Skeleton, dataObject, "besurel.as", AssetDataType.ASCII);
```

![besurel.as](example/img/besurel-skeleton.gif)

### ``.BM >> .OBJ >> Cinema 4D``

```cs
ChaosAsset chaosAsset = new ChaosAsset();
var dataObject = chaosAsset.Import(AssetType.Mesh, "besurel.bm", AssetDataType.Binary);
chaosAsset.Export(AssetType.Mesh, dataObject, "besurel.obj", AssetDataType.OBJ);
```

![besurel.obj](example/img/besurel-obj.png)

### ``.TEX >> .PNG``

```cs
ChaosAsset chaosAsset = new ChaosAsset();
var texture = chaosAsset.Import(AssetType.Texture, "besurel.tex", AssetDataType.Binary);
Bitmap bmp = texture.BitmapFrames[0]; // non animated textures are exported as Frame 0
bmp.Save("besurel.png", ImageFormat.Png);
```

<details>
  <summary>Besurel Texture</summary>
    
  ![besurel.png](example/besurel.png)
  
</details>
<details>
  <summary>Animated Texture</summary>
    
  ![example-animated-texture](https://user-images.githubusercontent.com/39301116/110999776-2fbf0600-8381-11eb-8134-0d0babced07a.gif)
  
</details>


## ``MAP ATTRIBUTES``
![attribute-all.png](example/map-attributes/attribute-all.png)

### ``.WTR >> .PNG``

```cs
ChaosMap chaosMap = new ChaosMap();
bool colorizeAttributes = true;
var dataObject = chaosMap.Import(ContentType.WorldTerrain, ContentDataType.Binary, "Dratan.wtr", colorizeAttributes);

// save attributes as merged bitmap
Bitmap bmpMerged = dataObject.AttributeBitmap.Layers.Merged;
bmpMerged.Save("attribute-combined.png", ImageFormat.Png);
```
<img src="/example/map-attributes/attribute-combined.png" alt="COMBINED" height="512" width="512"/>

```cs
ChaosMap chaosMap = new ChaosMap();
bool colorizeAttributes = true;
var dataObject = chaosMap.Import(ContentType.WorldTerrain, ContentDataType.Binary, "Dratan.wtr", colorizeAttributes);

// save single attribute
Bitmap bmpUnwalkable = dataObject.AttributeBitmap.Layers.MATT_UNWALKABLE;
bmpUnwalkable.Save("attribute-unwalkable.png", ImageFormat.Png);
```

<img src="/example/map-attributes/attribute-unwalkable.png" alt="MATT_UNWALKABLE" height="512" width="512"/>

### ``.SAT >> .PNG``

```cs
// sat contains only raw attribute bytes, there is no header defining version or map size so we need to do it manually
dynamic settings = new ExpandoObject();
settings.Width = 3072;
settings.Height = 3072;
settings.IsNew = true;                       // sat version
settings.SeparateLayers = true;              // whether create only merged bitmap or also separated layers

var dataObject = chaosMap.Import(ContentType.ServerAttributeMap, ContentDataType.Binary, "Dratan_3072_3072_0_1F.sat", settings);

// save single attribute
Bitmap bmpUnwalkable = dataObject.Layers.MATT_UNWALKABLE;
bmpUnwalkable.Save("attribute-unwalkable.png", ImageFormat.Png);

// save all merged attributes
Bitmap bmpMerged = dataObject.Layers.Merged;
bmpMerged.Save("attribute-combined.png", ImageFormat.Png);
```

### ``ATTRIBUTE COLORS``
<img src="https://www.codeapi.io/initials/%20?background=FFFFFFFF&shape=rounded" alt="#FFFFFFFF" height="15" width="15"/> `MATT_WALKABLE` <br/>
<img src="https://www.codeapi.io/initials/%20?background=FF000000&shape=rounded" alt="#FF000000" height="15" width="15"/> `MATT_UNWALKABLE` <br/>
<img src="https://www.codeapi.io/initials/%20?background=FF32CD32&shape=rounded" alt="#FF32CD32" height="15" width="15"/> `MATT_PEACE` <br/>
<img src="https://www.codeapi.io/initials/%20?background=FFFFA07A&shape=rounded" alt="#FFFFA07A" height="15" width="15"/> `MATT_FREEPKZONE` <br/>
<img src="https://www.codeapi.io/initials/%20?background=FFCD5C5C&shape=rounded" alt="#FFCD5C5C" height="15" width="15"/> `MATT_WAR` <br/>
<img src="https://www.codeapi.io/initials/%20?background=FFEE82EE&shape=rounded" alt="#FFEE82EE" height="15" width="15"/> `MATT_STAIR_UP` <br/>
<img src="https://www.codeapi.io/initials/%20?background=FFF5DEB3&shape=rounded" alt="#FFEE82EE" height="15" width="15"/> `MATT_STAIR_DOWN` <br/>
<img src="https://www.codeapi.io/initials/%20?background=FFFFE4E1&shape=rounded" alt="#FFFFE4E1" height="15" width="15"/> `MATT_PRODUCT_PUBLIC` <br/>
<img src="https://www.codeapi.io/initials/%20?background=FF800000&shape=rounded" alt="#FF800000" height="15" width="15"/> `MATT_PRODUCT_PRIVATE` <br/>


## Contributors
* [Karmel0x](https://github.com/Karmel0x) - his knowledge and helping hand <img align="left" src="https://cdn.frankerfacez.com/emoticon/250614/1" width="28px" height="22px">
