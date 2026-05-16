# DevState.md

## 1. 프로젝트 개요

- 프로젝트명: 2D Dungeon Survivor
- 장르: 2D 가로형 생존 로그라이크 / 뱀서류
- 엔진: Unity 6000.3.10f1
- 메인 씬: Assets/_Project/Scenes/Main.unity
- 개발 기준: 정식 출시 가능 구조 지향
- 그래픽 방향: 도트 기반 다크 판타지, Stoneshard풍 참고
- 현재 목표: 5분 MVP 안정화 후 10분 수직 슬라이스 확장
- 현재 개발 방식:
  - 단순 1개 파일 수정은 ChatGPT에서 전체 복붙 코드로 진행
  - 여러 파일 수정 / 구조 변경 / 리팩터링은 Antigravity를 활용
  - 문서 정리는 ChatGPT가 작성하고, Antigravity는 코드 수정 보조로 사용

---

## 2. 현재 진행 상태

현재 프로젝트는 단순 프로토타입 단계를 넘었고, **5분 MVP 안정화 후 10분 수직 슬라이스 초입 단계**에 있다.

현재 완성도 판단:

- 단순 프로토타입: 완료
- 5분 MVP: 약 85%
- 10분 수직 슬라이스: 약 45%
- 출시 기준 베이스: 약 30%

현재 핵심 루프는 아래까지 구현되어 있다.

```text
시작 화면
→ 게임 시작
→ 플레이어 이동
→ 몬스터 스폰
→ 자동 근접 공격
→ 몬스터 처치
→ 경험치 획득
→ 레벨업 선택
→ 능력치 강화 또는 무기 해금
→ Magic Bolt / Holy Aura 해금 가능
→ 중간보스 등장
→ 중간보스 처치
→ 유물 선택
→ 보유 유물 HUD 표시
→ 플레이어 사망
→ 게임오버 결과 화면
→ Retry
```

현재 단계의 핵심은 새 시스템을 무리하게 늘리는 것이 아니라, **5분 MVP 통합 테스트와 10분 수직 슬라이스 확장 기반 정리**다.

---

## 3. 최근 변경 사항

### 3.1 GameOver 결과 화면 개선 완료

- GameFlowManager.cs에 PlayerExp, WaveManager, GameTimer, GameOverText 참조 추가 완료
- 플레이어 사망 시 GameOverCanvas가 활성화됨
- GameOverText에 아래 정보 표시 가능:
  - Survival Time
  - Level
  - Wave
- GameOverCanvas/GameOverPanel/Text (TMP)를 GameFlowManager의 Game Over Text 필드에 연결 완료
- Retry 버튼 구조 유지

---

### 3.2 UI 연결 정리 완료

LevelUpUI 연결 정리 완료:

```text
Damage Button Text
Attack Speed Button Text
Attack Range Button Text
```

RelicSelectUI 연결 정리 완료:

```text
Relic Button Text 01
Relic Button Text 02
Relic Button Text 03
```

HUDCanvasUI 연결 및 표시 정리 완료:

```text
Relic Text → HUDPanel/RelicText
Relic Title → RELICS
Empty Relic Text → RELICS
```

현재 Play 화면에서 `RELICS` 텍스트가 정상 표시되는 것을 확인했다.

---

### 3.3 WaveManager + EnemySpawner 난이도 연동 완료

Antigravity를 사용하여 WaveManager.cs와 EnemySpawner.cs를 수정했다.

완료 내용:

- 웨이브가 오를수록 일반 몬스터 스폰 간격이 감소하도록 구조 추가
- 웨이브가 오를수록 최대 몬스터 수가 증가하도록 구조 추가
- EnemySpawner의 기존 Spawn Interval / Max Enemies 값은 1웨이브 기준값으로 유지
- WaveManager가 없을 경우 기존 EnemySpawner 기본값으로 fallback 되도록 구성
- 기존 EnemySpawnEntry / Spawn Table 구조 유지
- 중간보스 스폰 구조 유지
- 새 Manager / 새 Canvas / 새 UI Script 생성 없음

WaveManager에 추가된 Spawn Difficulty 항목:

```text
Spawn Interval Decrease Per Wave: 0.05
Min Spawn Interval: 0.5
Max Enemies Increase Per Wave: 2
Max Enemies Cap: 50
```

---

### 3.4 중간보스 체감 보강 완료

Antigravity를 사용하여 기존 중간보스 구조를 유지한 상태에서 1차 보강을 진행했다.

최종 Inspector 설정:

WaveManager:

```text
Mid Boss Health Multiplier: 10
Mid Boss Damage Multiplier: 1.7
Mid Boss Exp Multiplier: 4
```

Enemy_Bat_01 / Enemy_Ghoul_01 프리팹의 EnemyHealth:

```text
Mid Boss Scale: 1.5
Use Mid Boss Tint: 체크
Mid Boss Tint Color: 주황 계열
```

의도:

```text
중간보스가 일반 몬스터보다 더 단단함
중간보스가 일반 몬스터보다 더 위협적임
중간보스가 크기와 색상으로 구분됨
중간보스 처치 후 기존 유물 선택 UI 흐름 유지
```

---

### 3.5 Holy Aura 무기 추가 완료

Antigravity를 사용하여 세 번째 무기인 Holy Aura를 추가했다.

완료 내용:

- PlayerHolyAuraAutoAttack.cs 신규 생성
- LevelUpUI.cs에 Holy Aura 해금 처리 추가
- UpgradeData의 기존 WeaponUnlock 구조 재사용
- weaponId는 `holy_aura` 사용
- Holy Aura는 발사체 없이 플레이어 주변 범위 내 적에게 주기적으로 피해를 주는 지속 범위 공격
- Damage / Attack Speed / Attack Range 강화가 Holy Aura에도 적용되도록 확장
- 기존 Magic Bolt 해금 구조 유지
- 새 Manager / 새 Canvas / 새 UI Script 생성 없음

현재 무기 구조:

```text
기본 시작 무기:
- Sword Slash

획득형 무기:
- Magic Bolt
- Holy Aura
```

---

### 3.6 Brute 적 추가 완료

코드 수정 없이 Unity Prefab 작업으로 신규 적 `Enemy_Brute_01`을 추가했다.

완료 내용:

- `Enemy_Ghoul_01`을 복제하여 `Enemy_Brute_01` 생성
- Brute는 느리지만 체력이 높은 탱커형 적
- 기존 EnemyHealth / EnemyMovement / EnemyContactDamage 구조 재사용
- 기존 EnemySpawner Spawn Table 구조 재사용
- 새 스크립트 생성 없음

Enemy_Brute_01 기준 설정:

```text
Prefab: Assets/_Project/Prefabs/Enemies/Enemy_Brute_01.prefab
Scale: 1.15 / 1.15 / 1
Layer: Enemy
Max Health: 80
Exp Reward: 10
Move Speed: 약 1.3 권장
Contact Damage: 14
```

현재 적 역할 구분:

```text
Enemy_Ghoul_01:
- 일반 추적형

Enemy_Bat_01:
- 빠른 압박형

Enemy_Brute_01:
- 느리지만 단단한 탱커형
```

---

## 4. 현재 완료된 핵심 기능

### 4.1 플레이어

- PlayerController 기반 이동 구현
- Rigidbody2D 기반 이동
- PlayerHealth 기반 체력 / 방어 / 피격 / 무적 / 넉백 구현
- PlayerExp 기반 경험치 / 레벨 시스템 구현
- PlayerPickupRange 기반 경험치 자동 흡수 구현
- PlayerRelicEffects 기반 유물 효과 적용 구조 구현
- PlayerMagicBoltAutoAttack 컴포넌트 추가 완료
- PlayerHolyAuraAutoAttack 컴포넌트 추가 완료

Player에는 현재 다음 주요 컴포넌트가 붙어 있음:

```text
SpriteRenderer
Rigidbody2D
BoxCollider2D
PlayerController
PlayerMeleeAutoAttack
PlayerHealth
PlayerExp
PlayerPickupRange
PlayerRelicEffects
PlayerMagicBoltAutoAttack
PlayerHolyAuraAutoAttack
```

---

### 4.2 전투

- PlayerMeleeAutoAttack 기반 기본 근접 자동공격 구현
- MeleeSlashVisual 기반 임시 근접 공격 시각 효과 구현
- 적 피격 처리 구현
- 적 넉백 구현
- 적 피격 시 색 변화 및 스케일 변화 구현
- 치명타 확률 강화 구조 구현
- Magic Bolt 해금형 보조 무기 구조 추가 완료
- MagicBoltProjectile 발사체 스크립트 추가 완료
- MagicBolt 프리팹 추가 완료
- Holy Aura 해금형 지속 범위 공격 구조 추가 완료
- Damage / Attack Speed / Attack Range 강화가 근접 무기, Magic Bolt, Holy Aura에 적용되도록 확장 완료

현재 무기 구조:

```text
기본 시작 무기:
- Sword Slash / PlayerMeleeAutoAttack

획득형 무기:
- Magic Bolt / PlayerMagicBoltAutoAttack
- Holy Aura / PlayerHolyAuraAutoAttack
```

무기 역할:

```text
Sword Slash:
- 기본 근접 단발 공격

Magic Bolt:
- 원거리 자동 발사 공격

Holy Aura:
- 플레이어 주변 지속 범위 공격
```

Magic Bolt와 Holy Aura는 시작부터 활성화되지 않고, 레벨업 선택지에서 각각 선택해야 해금된다.

---

### 4.3 몬스터

- EnemySpawner 존재
- WaveManager와 연동됨
- EnemySpawnEntry 기반 몬스터 Spawn Table 사용
- 몬스터별 Spawn Weight 설정 가능
- EnemyMovement 기반 플레이어 추적 구현
- EnemyHealth 기반 체력 / 피격 / 사망 / 드랍 / 중간보스 처리 구현
- EnemyContactDamage 기반 접촉 피해 구현
- 구울 / 박쥐 / 브루트 프리팹 사용 가능
- 기존 빨간 박스 Enemy는 Spawn Table에서 제외하면 더 이상 스폰되지 않음
- WaveManager + EnemySpawner 난이도 연동 완료
- 웨이브별 스폰 간격 감소 / 최대 적 수 증가 구조 완료
- 중간보스 체력 / 피해량 / 시각 구분 1차 보강 완료

현재 주요 적 프리팹:

```text
Assets/_Project/Prefabs/Enemies/Enemy.prefab
Assets/_Project/Prefabs/Enemies/Enemy_Bat_01.prefab
Assets/_Project/Prefabs/Enemies/Enemy_Ghoul_01.prefab
Assets/_Project/Prefabs/Enemies/Enemy_Brute_01.prefab
```

현재 권장 Spawn Table 비율:

```text
Enemy_Ghoul_01: 5
Enemy_Bat_01: 2
Enemy_Brute_01: 1
```

---

### 4.4 경험치 / 성장

- ExpGem 드랍 및 획득 구현
- PlayerExp 시스템 구현
- 레벨 개념 구현
- 레벨업 시 LevelUpUI 오픈
- UpgradeData ScriptableObject 기반 강화 데이터 구조 구현
- 강화 선택지 3개 중 1개 선택 구조 구현
- 기본 강화 8개 + 무기 해금 2개 구성 완료

현재 강화 목록:

```text
Upgrade_Damage
Upgrade_AttackSpeed
Upgrade_AttackRange
Upgrade_MaxHealth
Upgrade_MoveSpeed
Upgrade_PickupRange
Upgrade_Defense
Upgrade_CriticalChance
Upgrade_MagicBolt
Upgrade_HolyAura
```

현재 UpgradeData 타입:

```text
Damage
AttackSpeed
AttackRange
MaxHealth
MoveSpeed
PickupRange
Defense
CriticalChance
WeaponUnlock
```

무기 해금 ID:

```text
Magic Bolt: magic_bolt
Holy Aura: holy_aura
```

---

### 4.5 유물

- RelicData ScriptableObject 기반 유물 데이터 구조 구현
- RelicSelectUI 기반 유물 3택 선택 구현
- 중간보스 처치 후 유물 선택 UI 오픈
- PlayerRelicEffects 기반 유물 효과 적용
- 보유 유물 HUD 표시 구조 구현
- HUDCanvasUI의 RelicText 표시 정상 확인 완료

현재 유물 목록:

```text
Relic_BerserkerFang
Relic_IronCharm
Relic_BloodSigil
Relic_HunterEye
Relic_GraveMagnet
```

현재 유물 효과:

```text
Berserker Fang:
- 체력이 일정 비율 이하일 때 공격 속도 증가

Iron Charm:
- 방어력 증가

Blood Sigil:
- 적 처치 시 일정 확률로 체력 회복

Hunter Eye:
- 치명타 관련 강화

Grave Magnet:
- 아이템 획득 범위 증가
```

---

### 4.6 아이템

- ExpGem 구현
- SmallHeal 구현
- SmallHeal은 ExpGem과 겹치지 않도록 드랍 위치 오프셋 적용 완료
- 중간보스는 SmallHeal을 항상 드랍하도록 설정 가능

현재 아이템 프리팹:

```text
Assets/_Project/Prefabs/Items/ExpGem.prefab
Assets/_Project/Prefabs/Items/SmallHeal.prefab
```

---

### 4.7 UI

현재 UI 구조:

```text
StartMenuCanvas
GameOverCanvas
LevelUpCanvas
RelicSelectUI
HUDCanvas
GameTimer
```

현재 UI 기능:

- 시작 화면 존재
- GAME START 버튼으로 게임 시작 가능
- HUDCanvas 존재
- HP 표시
- EXP 표시
- Wave 표시
- Timer 표시
- RelicText 표시
- LevelUpCanvas 존재
- 레벨업 선택 UI 구현
- RelicSelectUI 존재
- 유물 선택 UI 구현
- GameOverCanvas 존재
- GameOver 결과 텍스트 표시 구조 구현
- Retry 버튼으로 재시작 가능

정식 HUD 작업 기준:

```text
HUDCanvas
HUDPanel
HUDCanvasUI
```

사용하지 말아야 할 기준:

```text
HUDDebugUI
OnGUI 기반 Debug UI
```

HUDDebugUI는 디버그/보류용이며 신규 UI 작업의 기준으로 사용하지 않는다.

---

### 4.8 맵 / 배경

- GroundGrid 존재
- GroundTilemap 존재
- StageTileGenerator 존재
- 타일 자동 생성 구조 구현
- 여러 바닥 타일을 랜덤 배치하는 구조 존재
- 현재 타일 퀄리티는 임시 상태
- 추후 아트 단계에서 직접 제작한 타일로 교체 예정

---

## 5. 현재 주요 Scene 구조

현재 Main Scene 핵심 구조:

```text
Main Camera
Global Light 2D
Player
  - MeleeSlashVisual
EnemySpawner
WaveManager
RelicSelectUI
  - RelicSelectPanel
    - TitleText
    - InfoText
    - RelicButton_01
    - RelicButton_02
    - RelicButton_03
EventSystem
StageTileGenerator
GroundGrid
  - GroundTilemap
GameFlowManager
StartMenuCanvas
  - StartPanel
    - StartButton
GameOverCanvas
  - GameOverPanel
    - Text (TMP)
    - RetryButton
LevelUpCanvas
  - LevelUpPanel
    - TitleText
    - PendingText
    - DamageButton
    - AttackSpeedButton
    - AttackRangeButton
GameTimer
HUDCanvas
  - HUDPanel
    - HpSlider
    - HpText
    - ExpSlider
    - ExpText
    - WaveText
    - RelicText
  - TimerText
PauseManager
```

---

## 6. 현재 주요 스크립트 구조

```text
Assets/_Project/Scripts/Core
- CameraFollow2D.cs
- GameFlowManager.cs
- GameTimer.cs
- StageTileGenerator.cs

Assets/_Project/Scripts/Data
- UpgradeData.cs

Assets/_Project/Scripts/Debug
- HUDDebugUI.cs

Assets/_Project/Scripts/Editor
- DevSnapshotTool.cs
- EnemySpawnerEditor.cs

Assets/_Project/Scripts/Enemy
- EnemyContactDamage.cs
- EnemyHealth.cs
- EnemyMovement.cs

Assets/_Project/Scripts/Items
- ExpGem.cs
- SmallHealPickup.cs

Assets/_Project/Scripts/Player
- PlayerController.cs
- PlayerExp.cs
- PlayerHealth.cs
- PlayerPickupRange.cs

Assets/_Project/Scripts/Projectiles
- MagicBoltProjectile.cs

Assets/_Project/Scripts/Relics
- PlayerRelicEffects.cs
- RelicData.cs

Assets/_Project/Scripts/Spawning
- EnemySpawner.cs
- WaveManager.cs

Assets/_Project/Scripts/UI
- HUDCanvasUI.cs
- LevelUpUI.cs
- RelicSelectUI.cs

Assets/_Project/Scripts/Weapons
- PlayerMeleeAutoAttack.cs
- PlayerMagicBoltAutoAttack.cs
- PlayerHolyAuraAutoAttack.cs
```

---

## 7. 현재 남은 리스크

### 7.1 기능 검증 리스크

전체 통합 테스트는 아직 완전히 고정되지 않았다.

확인 필요:

```text
1. Magic Bolt가 레벨업 선택지에 정상 출현하는지
2. Magic Bolt 선택 시 PlayerMagicBoltAutoAttack이 정상 활성화되는지
3. Magic Bolt가 적을 향해 발사되는지
4. Magic Bolt가 적에게 피해를 주는지
5. Holy Aura가 레벨업 선택지에 정상 출현하는지
6. Holy Aura 선택 시 PlayerHolyAuraAutoAttack이 정상 활성화되는지
7. Holy Aura가 주변 적에게 주기적으로 피해를 주는지
8. Damage / Attack Speed / Attack Range 강화가 세 무기 모두에 적용되는지
9. Brute가 Spawn Table에서 정상 스폰되는지
10. Brute가 느리지만 단단한 적으로 체감되는지
11. 중간보스가 5분 시점에 정상 등장하는지
12. 중간보스가 일반 몬스터보다 단단하고 구분되는지
13. 중간보스 처치 후 유물 선택 UI가 정상 오픈되는지
14. 유물 선택 후 HUD에 보유 유물이 정상 표시되는지
15. 게임오버 후 Survival Time / Level / Wave가 표시되는지
16. Retry가 정상 작동하는지
```

---

### 7.2 UI 리스크

현재 UI는 기능 연결은 상당 부분 정리되었지만, 출시형 배치와 미적 완성도는 아직 부족하다.

남은 정리 필요:

```text
1. GameOverPanel 텍스트 위치 / 크기 조정
2. HUD 전체 배치 미세 조정
3. RelicText가 HP / EXP / Wave와 겹치지 않도록 최종 배치
4. Canvas Scaler 기준 확인
5. 해상도 변경 시 UI 깨짐 여부 확인
6. LevelUp 선택지 가독성 개선
7. RelicSelect 선택지 가독성 개선
```

주의:

```text
새 HUD Canvas를 만들지 말 것.
기존 HUDCanvas / HUDPanel / HUDCanvasUI를 기준으로 수정할 것.
```

---

### 7.3 웨이브 / 난이도 리스크

웨이브 난이도 연동은 구현되었으나, 체감 검증은 계속 필요하다.

확인 필요:

```text
1. 웨이브가 오를수록 몬스터 수가 실제로 많아지는지
2. 스폰 간격 감소가 체감되는지
3. Max Enemies Cap 50이 과하지 않은지
4. 중간보스 체력 배율 10이 적절한지
5. 중간보스 데미지 배율 1.7이 과하지 않은지
6. Brute의 체력 80 / 데미지 14 / 이동속도 1.3이 적절한지
7. 5분 MVP 기준으로 난이도가 너무 쉽거나 너무 어렵지 않은지
```

현재 설정:

```text
Wave Duration: 30
First Mid Boss Spawn Time: 300
Spawn Interval Decrease Per Wave: 0.05
Min Spawn Interval: 0.5
Max Enemies Increase Per Wave: 2
Max Enemies Cap: 50
Mid Boss Health Multiplier: 10
Mid Boss Damage Multiplier: 1.7
Mid Boss Exp Multiplier: 4
```

---

### 7.4 구조 리스크

현재 5분 MVP에서는 허용 가능하지만, 출시형 기준으로는 추후 정리해야 할 구조가 있다.

```text
1. 여러 UI가 Time.timeScale을 직접 제어할 가능성 있음
2. 적 / 투사체 / 드랍 아이템이 Instantiate / Destroy 기반임
3. 일부 스크립트에서 FindFirstObjectByType 사용
4. HUDDebugUI에 OnGUI 사용
5. 사운드 매니저 없음
6. 옵션 화면 없음
7. 저장 / 해금 시스템 없음
8. Object Pooling 없음
9. Sprite Atlas 없음
```

현재는 기능 안정화가 우선이므로 바로 고치지 않는다.  
10분 수직 슬라이스 이후 출시형 정리 단계에서 처리한다.

---

## 8. 앞으로의 개발 우선순위

## Phase 1. 5분 MVP 안정화

목표:

```text
한 판이 시작되고, 성장하고, 중간보스를 잡고, 유물을 얻고, 죽고, 다시 시작되는 흐름을 안정화한다.
```

현재 Phase 1에서 이미 완료한 항목:

```text
GameOver 결과 화면 개선
LevelUpUI 버튼 텍스트 연결 정리
RelicSelectUI 버튼 텍스트 연결 정리
HUD RelicText 표시 정리
WaveManager + EnemySpawner 난이도 연동
중간보스 체감 보강
Magic Bolt 추가
Holy Aura 추가
Brute 적 추가
```

남은 작업 순서:

### 1. 5분 MVP 통합 테스트

테스트 기준:

```text
1. 시작 화면에서 Game Start 가능
2. 플레이어 이동 가능
3. 적 스폰 정상
4. 적 처치 가능
5. ExpGem 획득 가능
6. 레벨업 선택 가능
7. Magic Bolt 해금 가능
8. Magic Bolt 발사 가능
9. Holy Aura 해금 가능
10. Holy Aura 범위 피해 가능
11. Brute 스폰 가능
12. 중간보스 등장
13. 중간보스가 일반 몬스터보다 단단하고 구분됨
14. 중간보스 처치 가능
15. 유물 선택 가능
16. HUD에 유물 표시
17. 플레이어 사망 시 GameOver 결과 화면 표시
18. Retry 정상 작동
19. Console 빨간 에러 없음
```

---

### 2. 밸런스 1차 조정

통합 테스트 후 아래를 조정한다.

```text
1. Magic Bolt 등장 타이밍
2. Magic Bolt 데미지 / 발사속도 / 사거리
3. Holy Aura 데미지 / 주기 / 범위
4. Brute 체력 / 이동속도 / 데미지 / Spawn Weight
5. 중간보스 체력 배율
6. 중간보스 데미지 배율
7. 적 스폰 간격 감소량
8. 최대 적 수 증가량
9. Max Enemies Cap
```

---

### 3. UI 표시 품질 정리

대상:

```text
HUD 위치
GameOver 결과 텍스트 위치
RelicText 크기 / 정렬
LevelUp 선택지 가독성
RelicSelect 선택지 가독성
```

---

## Phase 2. 재미 체감 보강

목표:

```text
반복 플레이가 덜 단조롭게 느껴지도록 전투와 웨이브 변화를 강화한다.
```

작업 순서:

### 1. 적 출현 비율 변화

개선 방향:

```text
초반:
- Ghoul 중심

중반:
- Bat 비율 증가
- Brute 소량 등장

중간보스 이후:
- Brute 비율 소폭 증가
- 물량 압박 증가
```

---

### 2. 강화 밸런스 조정

확인할 것:

```text
1. Damage Up 가치
2. Attack Speed 가치
3. Attack Range 가치
4. Max Health 가치
5. Defense 가치
6. Critical Chance 가치
7. Magic Bolt 해금 가치
8. Holy Aura 해금 가치
```

---

### 3. 중간보스 보상 체감 강화

개선 방향:

```text
1. 중간보스 등장 피드백
2. 중간보스 처치 피드백
3. 유물 선택으로 이어지는 보상감 강화
```

---

## Phase 3. 10분 수직 슬라이스 확장

목표:

```text
출시형 게임의 축소판을 만든다.
```

필요 기능:

```text
1. 10분 보스 추가
2. 유물 3~5개 추가
3. 강화 선택지 추가 또는 밸런스 정리
4. 결과 화면 보강
5. 웨이브 패턴 강화
6. 전투 피드백 강화
```

예상 추가 콘텐츠:

```text
무기:
- 현재 3종 구조 유지
- 필요 시 이후 4번째 무기 검토

적:
- 현재 Ghoul / Bat / Brute 3종 구조
- 이후 원거리 또는 돌진형 적 검토

유물:
- 공격 계열
- 방어 계열
- 흡수/성장 계열
```

---

## Phase 4. 출시형 기반 정리

목표:

```text
확장해도 무너지지 않는 구조로 정리한다.
```

작업 후보:

```text
1. PauseManager 도입 또는 기존 PauseManager 정리
2. Object Pooling 도입
3. SoundManager 도입
4. 옵션 화면 추가
5. 저장 / 해금 시스템 추가
6. Sprite Atlas 적용
7. 해상도 대응
8. 빌드 테스트
9. 로그 정리
10. 에셋 교체
```

이 단계는 지금 바로 하지 않는다.  
5분 MVP와 10분 수직 슬라이스가 안정화된 뒤 진행한다.

---

## 9. Antigravity 사용 원칙

Antigravity는 문서 작성용보다 코드 수정 / 반복 수정 / 리팩터링 보조로 사용한다.

사용 원칙:

```text
1. 한 번에 하나의 기능만 시킨다.
2. 수정 전 관련 파일을 먼저 읽게 한다.
3. 기존 구조를 재사용하게 한다.
4. 새 Canvas, 새 Manager, 새 UI Script를 임의로 만들지 못하게 한다.
5. 수정 범위를 파일 수 기준으로 제한한다.
6. 수정 후 Console 에러 0개를 확인한다.
7. 수정 후 DevSnapshot_Expert.md와 GPT_CONTEXT.md를 다시 생성한다.
8. 변경 사항은 Git commit으로 남긴다.
```

작업 분담 기준:

```text
ChatGPT:
- 개발 방향 판단
- 우선순위 정리
- 문서 작성
- Antigravity 프롬프트 작성
- 결과 검토
- Unity 연결 순서 안내

Antigravity:
- 여러 파일 코드 수정
- 기존 코드 읽고 수정
- 반복 리팩터링
- 구조 변경 작업 보조

사용자:
- Unity Inspector 연결
- Play 테스트
- Console 에러 확인
- 체감 피드백 제공
```

Antigravity에 시킬 때 기본 지시 형식:

```text
목표:
수정할 기능:

반드시 지킬 것:
- 기존 구조 유지
- 새 시스템 임의 생성 금지
- 관련 파일 먼저 읽기
- 수정 파일 목록 제한
- 코드 수정 후 예상 결과 설명
- Console 에러 0개 기준

수정 대상:
- 파일 경로

완료 조건:
- Play 시 정상 동작
- Console 에러 없음
- 기존 기능 유지
```

즉시 검토가 필요한 경우:

```text
1. Console 에러 발생
2. 플레이가 멈춤
3. 기존 기능이 사라짐
4. 스크립트가 3개 이상 바뀜
5. 새 Manager / 새 Canvas / 새 구조가 생김
6. Time.timeScale, 저장, Pooling, Scene 전환 관련 수정
```

---

## 10. 다음 작업

현재 다음 작업은 아래 순서로 진행한다.

```text
1. 5분 MVP 통합 테스트
2. Magic Bolt 실제 플레이 확인
3. Holy Aura 실제 플레이 확인
4. Brute 스폰 및 체감 확인
5. 중간보스 체감 확인
6. GameOver 결과 화면 확인
7. Retry 확인
8. Console 에러 확인
9. 테스트 결과 기반 밸런스 1차 조정
```

지금 바로 진행할 1순위:

```text
5분 MVP 통합 테스트
```

테스트 후 판단할 것:

```text
Magic Bolt가 재미를 주는지
Holy Aura가 너무 강하거나 약하지 않은지
Brute가 전투에 변화를 주는지
중간보스가 너무 쉽거나 너무 질질 끌리지 않는지
웨이브 증가가 체감되는지
유물 보상이 체감되는지
GameOver 결과 화면이 정상인지
Retry가 정상인지
```

통합 테스트가 정상이라면 다음 개발 단계는 **밸런스 1차 조정** 또는 **10분 보스 준비**다.