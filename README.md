https://github.com/user-attachments/assets/6bc843de-dc90-44f8-9513-17783f144ae2

#### How it Works
Add `[Visualize]` attribute to any of the supported members. Visualize attributes only work in classes that extend from `Node`.

```cs
// Position and _privateField will always update in the visualization UI
[Visualize(nameof(Position), nameof(_prviateField))]
public partial class Player : Node
{
    // These members will be editable in-game
    [Visualize] string[] PublicProperty { get; set; }
    [Visualize] int _privateField;

    [Visualize]
    public void SomeMethod(int a, Color b, Quaterion c)
}

```

#### Supported Members

| Member Type       | Supported  | Example Types                                 | Additional Notes                                                      |
|-------------------|------------|-----------------------------------------------|-----------------------------------------------------------------------|
| **Numericals**    | ✅         | `int`, `float`, `double`                      | All numerical types are supported                                     |
| **Enums**         | ✅         | `Direction`, `Colors`                         | All enum types are supported                                          |
| **Booleans**      | ✅         | `bool`                                        |                                                                       |
| **Strings**       | ✅         | `string`                                      |                                                                       |
| **Color**         | ✅         | `Color`                                       |                                                                       |
| **Vectors**       | ✅         | `Vector2`, `Vector2I`, `Vector3`, `Vector3I`, `Vector4`, `Vector4I` |                                                 |
| **Quaternion**    | ✅         | `Quaternion`                                  |                                                                       |
| **NodePath**      | ✅         | `NodePath`                                    |                                                                       |
| **StringName**    | ✅         | `StringName`                                  |                                                                       |
| **Methods**       | ✅         |                                               | Method parameters support all listed types here                       |
| **Static Members**| ✅         |                                               | This includes static methods, fields, and properties                  |
| **Arrays**        | ✅         | `int[]`, `string[]`, `Vector2[]`              | Arrays support all listed types here                                  |
| **Lists**         | ✅         | `List<string[]>`, `List<Vector2>`             | Lists support all listed types here                                   |
| **Dictionaries**  | ✅         | `Dictionary<List<Color[]>, Vector2>`          | Dictionaries support all listed types here                            |
| **Structs**       | ❌         |                                               | Currently disabled as very broken right now                           |
| **Classes**       | ❌         |                                               | Currently disabled as very broken right now                           |
| **Records**       | ❌         | `record`                                      | I couldn't get them to work for some reason                           |
| **Godot Classes** | ❌         | `Node`, `PointLight2D`                        | I'm not even sure how this would work                                 |
