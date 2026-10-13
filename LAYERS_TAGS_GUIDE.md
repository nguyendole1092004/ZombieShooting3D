# 🏷️ Unity Layers & Tags - Hướng dẫn nhanh

## Tags là gì?

**Tags** là nhãn để đánh dấu GameObjects, giúp tìm kiếm chúng trong code.

### Ví dụ trong game:
```csharp
// Tìm Player
GameObject player = GameObject.FindGameObjectWithTag("Player");

// Check xem có phải zombie không
if (hit.transform.CompareTag("Enemy")) {
    // Đây là zombie!
}
```

---

## Layers là gì?

**Layers** dùng để phân loại GameObjects, ảnh hưởng đến:
1. **Physics Collision** - Objects nào va chạm với nhau
2. **Raycast** - Chỉ bắn vào objects ở layer nhất định
3. **Camera Culling** - Camera chỉ render một số layers

### Ví dụ trong game:
```csharp
// Check player có chạm đất không
bool isGrounded = Physics.CheckSphere(
    groundCheck.position, 
    groundDistance, 
    groundMask  // ← CHỈ check với layer "Ground"
);
```

---

## 📋 Cách tạo Tags & Layers

### Tạo Tags:

1. **Edit > Project Settings > Tags and Layers**
2. Tab **"Tags"**
3. Click nút **"+"**
4. Nhập tên tag: `Player`
5. Lặp lại để tạo tag `Enemy`

### Tạo Layers:

1. **Edit > Project Settings > Tags and Layers**
2. Tab **"Layers"**
3. Tìm Layer trống (User Layer 6, 7, 8...)
4. Click vào ô trống, nhập tên:
   - Layer 6: `Player`
   - Layer 7: `Enemy`
   - Layer 8: `Ground`

---

## 🎯 Assign Tags & Layers cho GameObjects

### Assign Tag:
1. Chọn GameObject trong Hierarchy
2. Ở đầu Inspector, có dropdown **"Tag"**
3. Chọn tag muốn gán (ví dụ: Player)

### Assign Layer:
1. Chọn GameObject trong Hierarchy
2. Ở đầu Inspector, có dropdown **"Layer"**
3. Chọn layer muốn gán (ví dụ: Ground)

---

## 🎮 Tags & Layers cần thiết cho game

### Tags (3 tags):
```
✓ Player    - Gán cho Player GameObject
✓ Enemy     - Gán cho tất cả Zombie prefabs
✓ MainCamera - Gán cho Main Camera (Unity tự tạo sẵn)
```

### Layers (3 layers):
```
✓ Player (Layer 6) - Gán cho Player GameObject
✓ Enemy (Layer 7)  - Gán cho tất cả Zombie prefabs
✓ Ground (Layer 8) - Gán cho Plane/Terrain (mặt đất)
```

---

## ⚙️ Layer Collision Matrix

Để tránh zombie va chạm với nhau, hoặc player đẩy player:

1. **Edit > Project Settings > Physics**
2. Cuộn xuống **"Layer Collision Matrix"**
3. **Uncheck** các ô:
   - Player vs Player (player không va chạm player khác)
   - Enemy vs Enemy (zombie không va chạm zombie khác)

**Giữ check:**
- Player vs Enemy (player va chạm zombie - OK)
- Player vs Ground (player đứng trên đất - OK)
- Enemy vs Ground (zombie đi trên đất - OK)

---

## 🔍 Tại sao cần Ground Layer?

### Trong script PlayerMovement.cs:

```csharp
public LayerMask groundMask;  // Chỉ định layer nào là đất

void Update() {
    // Check player có đứng trên đất không
    isGrounded = Physics.CheckSphere(
        groundCheck.position,    // Vị trí dưới chân
        groundDistance,          // Bán kính check (0.4m)
        groundMask              // CHỈ check với layer "Ground"
    );
    
    // Nếu đứng trên đất → cho phép nhảy
    if (Input.GetButtonDown("Jump") && isGrounded) {
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
    }
}
```

**Không có Ground Layer:**
- Check sẽ va chạm với TẤT CẢ objects (zombie, walls, props...)
- Player có thể "nhảy trên zombie" 😅

**Có Ground Layer:**
- Check CHỈ với mặt đất
- Player chỉ nhảy được khi chạm đất thật sự ✅

---

## 🎯 Setup nhanh cho game

### Bước 1: Tạo Tags & Layers (5 phút)

```
Edit > Project Settings > Tags and Layers

TAGS:
  + Player
  + Enemy

LAYERS:
  Layer 6: Player
  Layer 7: Enemy
  Layer 8: Ground
```

### Bước 2: Gán cho GameObjects

**Ground (Plane):**
- Tag: Untagged (để mặc định)
- Layer: **Ground**

**Player:**
- Tag: **Player**
- Layer: **Player**

**Zombie prefabs:**
- Tag: **Enemy**
- Layer: **Enemy**

**Main Camera:**
- Tag: **MainCamera** (Unity tự set sẵn)
- Layer: Default (để mặc định)

### Bước 3: Physics Collision

```
Edit > Project Settings > Physics
Layer Collision Matrix:
  ☐ Player vs Player
  ☐ Enemy vs Enemy
```

---

## 🐛 Troubleshooting

### Zombie không đuổi player?
```
✓ Check Player có Tag "Player" chưa
✓ Check trong ZombieAI.cs có dòng:
  GameObject.FindGameObjectWithTag("Player")
```

### Player không nhảy được?
```
✓ Check Plane có Layer "Ground" chưa
✓ Check PlayerMovement > Ground Mask chọn layer Ground chưa
✓ Check GroundCheck position đúng không (dưới chân player)
```

### Lỗi "Layer 'Ground' doesn't exist"?
```
✓ Check đã tạo layer Ground trong Tags and Layers chưa
✓ Restart Unity Editor nếu vừa tạo layer mới
```

---

## 💡 Tips

### Kiểm tra Layer của GameObject:
```csharp
Debug.Log("Layer: " + gameObject.layer);
Debug.Log("Layer name: " + LayerMask.LayerToName(gameObject.layer));
```

### Kiểm tra Tag:
```csharp
Debug.Log("Tag: " + gameObject.tag);
```

### Chuyển đổi Layer sang LayerMask:
```csharp
int groundLayer = LayerMask.NameToLayer("Ground");  // 8
LayerMask groundMask = 1 << groundLayer;            // 256
```

---

## 📚 Tài liệu Unity

- [Tags](https://docs.unity3d.com/Manual/Tags.html)
- [Layers](https://docs.unity3d.com/Manual/Layers.html)
- [Layer-based collision detection](https://docs.unity3d.com/Manual/LayerBasedCollision.html)

---

## ✅ Checklist

Trước khi test game, kiểm tra:

- [ ] Đã tạo 2 tags: Player, Enemy
- [ ] Đã tạo 3 layers: Player, Enemy, Ground
- [ ] Player có tag "Player" và layer "Player"
- [ ] Zombie có tag "Enemy" và layer "Enemy"
- [ ] Plane có layer "Ground"
- [ ] Camera có tag "MainCamera"
- [ ] PlayerMovement.groundMask đã chọn layer Ground
- [ ] Layer Collision Matrix đã uncheck Player vs Player và Enemy vs Enemy

---

**Nếu setup đúng, game sẽ hoạt động mượt mà!** ✨
