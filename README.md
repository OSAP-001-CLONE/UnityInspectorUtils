# 🚀 Unity Inspector Utils

Unity Inspector Utils is a collection of powerful custom attributes designed to improve the Unity Inspector experience, making it more dynamic and user-friendly.

### Features

- **Enhanced Readability & Organization** – Keeps the Inspector clean by grouping and hiding fields dynamically.
- **Improved Workflow Efficiency** – Reduces the need for custom editor scripts, making development faster and more intuitive.
- **Easy to Use & Integrate** – Simply add attributes to your fields without modifying the Unity Editor itself.
- **Open-Source & Free to Use** – Completely free, with full access to the source code. You can modify or extend its features as needed.

### Compatibility

| Unity Version | Compatibility |
|:-------------:|:-------------:|
| **6000.0.32f1** |:heavy_check_mark:|
| **2022.3.47f1** |:heavy_check_mark:|
| **2022.3.20f1** |:heavy_check_mark:|
| **2022.3.10f1** |:heavy_check_mark:|


## How To Use

### Import

Go to release tab (https://github.com/qweasfjbv/UnityInspectorUtils/releases/tag/v1.0.0)
![image](https://github.com/user-attachments/assets/a4b43bbf-bd49-458f-b1c7-6ab6f8a21f5a)

Download the `.unitypackage` file and import it into Unity.

![image](https://github.com/user-attachments/assets/97a7d06c-8be0-492e-9e06-3222ba58deaa)


### Tab Groups

```cs
[TabGroup("Main", "Tab1")]
public int mainTab1Int;
public Vector3 mainTab1Vec;

[TabGroup("Sub", "Tab1")]
public int subTab1Int;
public float subTab1Float;

[TabGroup("Sub", "Tab2")]
public float subTab2Float;
public int subTab2Int;

[TabGroup("Main", "Tab2")]
public string mainTab2String;
public int mainTab2Int;

[TabGroup("Main", "Tab3")]
public string mainTab3String;
public int mainTab3Int;
```
<p align="center">
  <image src="https://github.com/user-attachments/assets/2d434b83-a5fa-4de1-bf10-91bc13d77457" width="30%"></image>
  <image src="https://github.com/user-attachments/assets/355686a0-4b3e-4383-869f-64fd444af4a8" width="30%"></image>
  <image src="https://github.com/user-attachments/assets/14fdd170-9392-4fab-8d09-97ff512c49ab" width="30%"></image>
</p>

<br>

### Foldout Groups

```cs
[FoldoutGroup("Fold1")]
public float foldFloat1;
public float foldFloat2;

[FoldoutGroup("Fold2", 20, ColorType.Red)]
public int foldInt1;
public int foldInt2;
```

<p align="center">
  <image src="https://github.com/user-attachments/assets/a6d17366-17ba-40d2-9660-1ef4c5c27fb4" width="30%"></image>
  <image src="https://github.com/user-attachments/assets/74752a1e-f71b-4f8b-ae62-8fc3eadc28b8" width="30%"></image>
</p>

<br>

```cs
[TabGroup("Main", "Tab1")]
[FoldoutGroup("Tab1Fold")]
public float fold1Float;

[TabGroup("Main", "Tab2")]
[FoldoutGroup("Tab2Fold")]
public float fold2Float;
```

<p align="center">
  <image src="https://github.com/user-attachments/assets/a1406d47-8dcf-475f-8c1c-60cbd3dd1777" width="30%"></image>
  <image src="https://github.com/user-attachments/assets/f6eb1031-e732-43eb-931f-f897619d3bf4" width="30%"></image>
</p>

<br>

### If Attributes

```cs
public bool isReadonly;
[ReadOnlyIf(nameof(isReadonly))]
public int readonlyValue;

public bool isHide;
[HideIf(nameof(isHide))]
public float hideValue;

public bool isShow;
[ShowIf(nameof(isShow))]
public float showValue;
```

<p align="center">
  <image src="https://github.com/user-attachments/assets/8316857e-9865-4b7a-bad9-ae3437ac0424" width="30%"></image>
  <image src="https://github.com/user-attachments/assets/44ab6b9f-e84a-482e-8dce-4aad0a209002" width="30%"></image>
</p>

<br>

### Popup Option

```cs
private int[] intOptions = new int[] { 1, 2, 3 };
private float[] floatOptions = new float[] { 5.0f, 6.0f, 7.0f };
private string[] stringOptions = new string[] { "Option1", "Option2", "Option3" };

[PopupOption(nameof(intOptions))]
public int selectInt;

[PopupOption(nameof(floatOptions), 1)]
public float selectFloat;

[PopupOption(nameof(stringOptions), 2)]
public string selectString;
```

<p align="center">
  <image src="https://github.com/user-attachments/assets/f6ca510c-ac9a-4f9e-a461-0403ceb40de2" width="30%"></image>
  <image src="https://github.com/user-attachments/assets/88e5c14e-ff83-43b6-bfa9-ae410201eae4" width="30%"></image>
  <image src="https://github.com/user-attachments/assets/06fd8172-81c1-4f1b-9211-a7bf223ba83d" width="30%"></image>
</p>

<br>

### Button

```cs
public int FuncParam1;
public float FuncParam2;

[Button(nameof(FuncParam1), nameof(FuncParam2))]
public void Func(int param1, float param2)
{
  Debug.Log($"Func2 Executed! \nParam1 : {param1}, Param2 : {param2}");
}
```

<p align="center">
  <image src="https://github.com/user-attachments/assets/bba29437-8999-4060-b217-c04c41e8b529"></image>
  <image src="https://github.com/user-attachments/assets/51b0fb05-561d-403f-90b1-26f0517e7bd7"></image>
</p>


