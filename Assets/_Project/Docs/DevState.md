# DevState.md

## 0. 문서 목적

이 문서는 Unity 2D Dungeon Survivor 프로젝트의 현재 개발 상태를 기록하는 기준 문서다.

목적은 다음과 같다.

```text
1. 현재 구현된 기능을 정확히 기록한다.
2. 이미 해결된 문제와 아직 남은 문제를 구분한다.
3. 다음 작업 우선순위를 명확히 한다.
4. AI에게 현재 프로젝트 상태를 전달할 때 기준 자료로 사용한다.
5. 예전 MD 문서와 실제 프로젝트 상태가 어긋나는 문제를 방지한다.
```

이 문서는 `AGENTS.md`와 함께 사용한다.

```text
AGENTS.md:
- AI 개발 규칙
- 중복 시스템 생성 방지
- 코드 수정 원칙

DevState.md:
- 현재 개발 상태
- 완료된 기능
- 남은 문제
- 다음 작업 우선순위
```

---

## 1. 프로젝트 개요

- 프로젝트명: 2D Dungeon Survivor
- 장르: 2D 가로형 생존 로그라이크 / 뱀서류
- 개발 엔진: Unity
- 현재 Unity 버전: 6000.3.10f1
- 메인 씬: `Assets/_Project/Scenes/Main.unity`
- 개발 기준: 정식 출시 가능 구조 지향
- 현재 목표: 5분 MVP 안정화 후 10분 수직 슬라이스 확장
- 그래픽 방향: 도트 기반 다크 판타지, 스톤샤드풍 참고
- 개발 방식: 한 번에 하나의 기능만 추가하고, 기존 구조를 보존한다.

---

## 2. 현재 진행 단계

현재 프로젝트는 기초 기능을 하나씩 붙이는 초반 단계를 넘어섰다.

현재 상태는 다음과 같다.

```text
5분 MVP 핵심 루프 대부분 구현 완료
→ 5분 플레이 안정화 필요
→ 10분 수직 슬라이스 확장 준비 단계
```

현재 구현된 핵심 플레이 루프는 다음과 같다.

```text
게임 시작
→ 플레이어 이동
→ 몬스터 스폰
→ 자동 공격
→ 몬스터 처치
→ 경험치 획득
→ 레벨업 선택
→ 강화 적용
→ 웨이브 진행
→ 중간보스 등장
→ 유물 선택
→ 보유 유물 HUD 표시
→ 사망 시 게임오버
→ 재시작
```

현재 결론:

```text
이제 단순히 기능을 계속 추가하는 단계가 아니다.
우선 구조 충돌을 줄이고, 웨이브 체감과 성장 체감을 강화해야 한다.
```

---

## 3. 최근 주요 변경 사항

최근 반영된 주요 변경 사항은 다음과 같다.

```text
- WaveManager에 OnWaveChanged 이벤트 추가
- HUDCanvasUI에서 WaveManager.OnWaveChanged 구독/해제 추가
- 웨이브 상태 변경 즉시 HUD 반영
- RelicSelectUI에서 Random.Range 충돌 수정
- RelicSelectUI에서 UnityEngine.Random.Range 명시
- HUDCanvasUI에 보유 유물 표시 구조 추가
- RelicSelectUI에 OwnedRelics / OnOwnedRelicsChanged 구조 추가
- PauseManager 기반 일시정지 구조 일부 적용
- Magic Bolt 무기 구조 추가
- UpgradeData에 WeaponUnlock 흐름 추가
- SmallHeal과 ExpGem 드랍 위치 분리 개선
```

---

## 4. 현재 주요 폴더 구조

현재 프로젝트 기준 폴더 구조는 다음과 같다.

```text
Assets/_Project
├── Animations
├── Art
│   ├── Backgrounds
│   ├── Characters
│   ├── Effects
│   ├── Enemies
│   ├── Tiles
│   ├── UI
│   └── Weapons
├── Audio
│   ├── BGM
│   └── SFX
├── Docs
├── Materials
├── Prefabs
│   ├── Enemies
│   ├── Items
│   ├── Player
│   ├── Projectiles
│   └── UI
├── Scenes
├── ScriptableObjects
│   ├── Characters
│   ├── Enemies
│   ├── Relics
│   ├── Upgrades
│   └── Weapons
└── Scripts
    ├── Combat
    ├── Core
    ├── Data
    ├── Debug
    ├── Editor
    ├── Enemy
    ├── Items
    ├── Player
    ├── Projectiles
    ├── Relics
    ├── Spawning
    ├── UI
    └── Weapons
```

새 스크립트나 에셋을 만들 때는 이 구조를 기준으로 배치한다.

---

## 5. 현재 완료된 기능

---

### 5.1 플레이어

완료된 기능:

```text
- 플레이어 이동 가능
- 기사 도트 스프라이트 적용
- Rigidbody2D 기반 2D 이동 구조 사용
- 체력 시스템 존재
- 피격 가능
- 무적 시간 존재
- 넉백 가능
- 방어력 적용 가능
- 최대 체력 강화 가능
- 이동속도 강화 가능
- 아이템 획득 범위 강화 가능
```

관련 주요 스크립트:

```text
Assets/_Project/Scripts/Player/PlayerHealth.cs
Assets/_Project/Scripts/Player/PlayerExp.cs
Assets/_Project/Scripts/Player/PlayerPickupRange.cs
Assets/_Project/Scripts/Player/PlayerRelicEffects.cs
```

주의 사항:

```text
- PlayerHealth가 체력, 피격, 방어력의 기준이다.
- PlayerExp가 레벨과 경험치의 기준이다.
- PlayerPickupRange가 아이템 획득 범위의 기준이다.
- 새 체력 시스템이나 새 경험치 시스템을 만들지 않는다.
```

현재 평가:

```text
플레이어 기본 조작과 생존 구조는 구현 완료 상태다.
이제 이동 자체보다 피격감, 사망 처리, 성장 체감 보강이 중요하다.
```

---

### 5.2 전투 - 근접 자동 공격

완료된 기능:

```text
- 자동 근접 공격 구현
- 사거리 내 몬스터 탐색 가능
- 몬스터 피격 가능
- 몬스터 넉백 가능
- 공격력 강화 가능
- 공격속도 강화 가능
- 공격범위 강화 가능
- 치명타 확률 강화 가능
```

관련 주요 스크립트:

```text
Assets/_Project/Scripts/Weapons/PlayerMeleeAutoAttack.cs
```

현재 평가:

```text
근접 자동 공격은 현재 기본 전투의 중심이다.
기능은 작동하지만 타격감은 아직 최소 수준이다.
```

남은 개선 방향:

```text
- 공격 이펙트 강화
- 타격 사운드 추가
- 피격 순간 시각 효과 추가
- 카메라 흔들림 추가 검토
- 데미지 숫자 표시 검토
```

---

### 5.3 전투 - Magic Bolt 무기

완료된 기능:

```text
- Magic Bolt 자동 공격 스크립트 존재
- Magic Bolt Projectile 스크립트 존재
- Magic Bolt 프리팹 존재
- WeaponUnlock 강화로 해금 가능
- 공격력 / 공격속도 / 공격범위 강화 일부 연동
```

관련 주요 스크립트:

```text
Assets/_Project/Scripts/Weapons/PlayerMagicBoltAutoAttack.cs
Assets/_Project/Scripts/Projectiles/MagicBoltProjectile.cs
Assets/_Project/Scripts/UI/LevelUpUI.cs
```

현재 평가:

```text
무기 2개 구조로 확장되기 시작했다.
근접 무기만 있던 단조로움은 일부 해소되었다.
다만 무기별 개별 성장 구조는 아직 명확히 분리되지 않았다.
```

남은 확인 사항:

```text
- PlayerMagicBoltAutoAttack의 projectileSpawnPoint 연결 여부
- MagicBolt 프리팹 연결 여부
- Magic Bolt 해금 전에는 발사되지 않는지
- Magic Bolt 해금 후 정상 발사되는지
- 공격력 강화가 Magic Bolt에도 의도대로 적용되는지
- 공격속도 강화가 Magic Bolt에도 의도대로 적용되는지
- 공격범위 강화가 Magic Bolt에도 의도대로 적용되는지
```

주의 사항:

```text
- 다음 무기 추가 전 공통 강화와 개별 강화 기준을 정해야 한다.
- 새 WeaponManager 같은 큰 구조는 아직 만들지 않는다.
- 기존 구조를 유지하면서 필요한 만큼만 확장한다.
```

---

### 5.4 몬스터

완료된 기능:

```text
- 몬스터 체력 존재
- 몬스터 이동 존재
- 몬스터 접촉 데미지 존재
- 몬스터 피격 가능
- 몬스터 넉백 가능
- 몬스터 처치 가능
- ExpGem 드랍 가능
- SmallHeal 드랍 가능
```

관련 주요 스크립트:

```text
Assets/_Project/Scripts/Enemy/EnemyHealth.cs
Assets/_Project/Scripts/Enemy/EnemyMovement.cs
Assets/_Project/Scripts/Enemy/EnemyContactDamage.cs
```

현재 평가:

```text
기본 몬스터 루프는 작동한다.
다만 적 종류와 웨이브별 조합 변화는 아직 약하다.
```

남은 개선 방향:

```text
- 빠르고 약한 적
- 느리고 단단한 적
- 돌진형 적
- 원거리 적
- 중간보스 이후 강한 적 비중 증가
```

주의 사항:

```text
- EnemyHealth 수정 시 ExpGem / SmallHeal 드랍 구조를 같이 확인한다.
- EnemyMovement의 Separation 계산은 적 수가 많아질 경우 성능 테스트가 필요하다.
- 적 종류 추가 시 EnemySpawner의 Spawn Table 구조를 우선 사용한다.
```

---

### 5.5 아이템 / 드랍

완료된 기능:

```text
- ExpGem 드랍
- ExpGem 획득
- 경험치 증가
- SmallHeal 드랍
- SmallHeal 회복
- ExpGem과 SmallHeal 드랍 위치 분리 개선
```

관련 주요 스크립트:

```text
Assets/_Project/Scripts/Items/ExpGem.cs
Assets/_Project/Scripts/Items/SmallHealPickup.cs
Assets/_Project/Scripts/Enemy/EnemyHealth.cs
```

현재 평가:

```text
SmallHeal과 ExpGem 겹침 문제는 1차 개선 완료 상태다.
다만 시각적으로 SmallHeal과 ExpGem의 구분은 아직 더 강화할 필요가 있다.
```

남은 개선 방향:

```text
- SmallHeal 색상/스프라이트 차별화
- 획득 시 회복 이펙트 추가
- 회복량 표시 또는 짧은 피드백 추가
```

---

### 5.6 경험치 / 레벨업

완료된 기능:

```text
- 경험치 획득 가능
- 레벨업 가능
- 레벨업 UI 표시 가능
- 레벨업 시 3개 선택지 제공
- UpgradeData 기반 강화 구조 존재
- 강화 선택 후 게임 재개 가능
```

관련 주요 스크립트:

```text
Assets/_Project/Scripts/Player/PlayerExp.cs
Assets/_Project/Scripts/UI/LevelUpUI.cs
Assets/_Project/Scripts/Data/UpgradeData.cs
```

현재 강화 종류:

```text
- Damage Up
- Attack Speed Up
- Attack Range Up
- Max Health Up
- Move Speed Up
- Pickup Range Up
- Defense Up
- Critical Chance Up
- Weapon Unlock: Magic Bolt
```

현재 평가:

```text
5분 MVP 기준 성장 구조는 구현 완료로 본다.
다음 단계는 강화 선택의 체감과 밸런스 조정이다.
```

남은 개선 방향:

```text
- 강화 수치 밸런스 조정
- 선택지 설명 가독성 개선
- 선택 후 강화 체감 피드백 추가
- 무기별 개별 강화 구조 검토
```

주의 사항:

```text
- 새 강화 시스템을 만들지 않는다.
- UpgradeData 기반 구조를 유지한다.
- LevelUpUI에 모든 로직이 과도하게 쌓이지 않도록 주의한다.
```

---

### 5.7 유물

완료된 기능:

```text
- RelicData 기반 유물 구조 존재
- RelicSelectUI 존재
- 중간보스 보상으로 유물 선택 가능
- 3개 유물 선택지 제공
- 이미 보유한 유물 제외 가능
- 보유 유물 목록 유지
- 보유 유물 HUD 표시 가능
- 일부 유물 효과 구현 완료
```

관련 주요 스크립트:

```text
Assets/_Project/Scripts/UI/RelicSelectUI.cs
Assets/_Project/Scripts/Relics/RelicData.cs
Assets/_Project/Scripts/Player/PlayerRelicEffects.cs
Assets/_Project/Scripts/UI/HUDCanvasUI.cs
```

현재 유물 후보:

```text
- Berserker Fang
- Iron Charm
- Blood Sigil
- Hunter Eye
- Grave Magnet
```

현재 평가:

```text
유물 시스템은 1차 구현 완료 상태다.
보유 유물 표시도 HUD에 반영된 상태다.
남은 문제는 획득 연출, 효과 체감, 아이콘 표시다.
```

남은 개선 방향:

```text
- 유물 선택 시 효과음 추가
- 유물 획득 직후 짧은 안내 표시
- HUD에 텍스트뿐 아니라 아이콘 표시
- 유물 효과가 실제 플레이에서 체감되도록 수치 조정
```

주의 사항:

```text
- RelicSelectUI에 효과 로직을 계속 쌓지 않는다.
- 유물 효과는 PlayerRelicEffects에서 관리하는 방향을 우선한다.
- 보유 유물 표시는 HUDCanvasUI의 relicText 구조를 사용한다.
```

---

### 5.8 웨이브 / 스폰

완료된 기능:

```text
- EnemySpawner 존재
- Spawn Table 존재
- Enemy Pooling 존재
- WaveManager 존재
- 웨이브 증가 가능
- 웨이브 HUD 표시 가능
- Wave 변경 이벤트 존재
- 5분 중간보스 스폰 가능
```

관련 주요 스크립트:

```text
Assets/_Project/Scripts/Spawning/EnemySpawner.cs
Assets/_Project/Scripts/Spawning/WaveManager.cs
```

현재 평가:

```text
웨이브 시스템은 존재하지만 체감 변화가 아직 약하다.
단순 체력/데미지 증가만으로는 반복감이 생길 수 있다.
10분 수직 슬라이스로 가려면 웨이브별 적 조합 변화가 필요하다.
```

다음 개선 후보:

```text
- 웨이브별 maxEnemies 증가
- 웨이브별 spawnInterval 감소
- 웨이브별 적 Spawn Weight 변화
- 5분 중간보스 이후 난이도 상승
- 7~10분 구간에 강한 적 추가
```

주의 사항:

```text
- 새 Wave 시스템을 만들지 않는다.
- 새 EnemySpawner를 만들지 않는다.
- 기존 WaveManager와 EnemySpawner의 연결 구조를 먼저 확인한다.
```

---

### 5.9 UI

완료된 기능:

```text
- StartMenuCanvas 존재
- HUDCanvas 존재
- LevelUpCanvas 존재
- RelicSelectUI 존재
- GameOverCanvas 존재
- Retry 버튼 존재
- HP 표시
- EXP 표시
- Wave 표시
- 보유 유물 표시
```

관련 주요 스크립트:

```text
Assets/_Project/Scripts/Core/GameFlowManager.cs
Assets/_Project/Scripts/UI/HUDCanvasUI.cs
Assets/_Project/Scripts/UI/LevelUpUI.cs
Assets/_Project/Scripts/UI/RelicSelectUI.cs
```

현재 평가:

```text
기능적 UI는 구현되어 있다.
디자인은 아직 임시 상태다.
출시형 UI로 가려면 배치, 폰트, 색상, 아이콘, 피드백을 정리해야 한다.
```

남은 개선 방향:

```text
- HUD 배치 정리
- HP / EXP / Wave / Relic 시각 구분 강화
- 레벨업 선택지 가독성 개선
- 유물 선택 UI 연출 강화
- 시작 화면과 게임오버 화면 디자인 정리
```

주의 사항:

```text
- 새 HUDCanvas를 만들지 않는다.
- 새 LevelUpCanvas를 만들지 않는다.
- 새 RelicSelectCanvas를 만들지 않는다.
- Debug UI와 정식 HUD를 혼동하지 않는다.
```

---

### 5.10 맵 / 배경

완료된 기능:

```text
- Tilemap 기반 바닥 사용
- StageTileGenerator 존재
- 청크 기반 바닥 생성 구조 존재
- 여러 바닥 타일 랜덤 배치 가능
```

관련 주요 스크립트:

```text
Assets/_Project/Scripts/Core/StageTileGenerator.cs
```

현재 평가:

```text
맵은 기능적으로 무한/반복 배경에 가까운 구조를 갖추고 있다.
아트 품질은 아직 임시 상태다.
향후 직접 제작한 타일셋이나 구매 에셋으로 교체할 수 있다.
```

남은 개선 방향:

```text
- 바닥 타일 다양화
- 장식 오브젝트 배치
- 다크 판타지 분위기 강화
- 장애물 또는 지형 요소 추가 검토
```

---

### 5.11 게임 흐름

완료된 기능:

```text
- 시작 메뉴 존재
- 게임 시작 가능
- HUD 표시 가능
- 게임오버 가능
- 재시작 가능
- 일시정지 구조 일부 존재
```

관련 주요 스크립트:

```text
Assets/_Project/Scripts/Core/GameFlowManager.cs
Assets/_Project/Scripts/Core/GameTimer.cs
Assets/_Project/Scripts/Core/PauseManager.cs
Assets/_Project/Scripts/Player/PlayerHealth.cs
```

현재 평가:

```text
게임 흐름은 기능적으로 이어져 있다.
다만 Time.timeScale 제어가 여러 곳에 남아 있을 경우 충돌 가능성이 있다.
PauseManager 중심으로 정리하는 작업이 필요하다.
```

---

## 6. 현재 남은 핵심 문제

---

### 6.1 Pause 구조 정리 필요

현재 `PauseManager`가 존재하고 LevelUpUI, RelicSelectUI 등에서 사용 중이다.

다만 일부 스크립트에 `Time.timeScale` 직접 제어가 남아 있을 가능성이 있다.

개선 방향:

```text
- 모든 일시정지 요청을 PauseManager로 통합
- GameOver, LevelUp, RelicSelect가 동시에 pause를 요청해도 충돌하지 않게 유지
- PlayerHealth.Die()의 Time.timeScale 직접 제어 여부 확인
- GameFlowManager와 PauseManager의 역할 분리
```

우선순위:

```text
높음
```

이유:

```text
UI가 늘어날수록 Time.timeScale 직접 제어는 충돌 원인이 된다.
출시형 구조로 가려면 pause 제어는 한 곳에서 관리하는 것이 좋다.
```

---

### 6.2 웨이브 체감 강화 필요

현재 웨이브는 존재하지만 플레이 체감상 변화가 약할 수 있다.

개선 방향:

```text
- 웨이브별 적 수 증가
- 웨이브별 스폰 간격 감소
- 웨이브별 적 조합 변화
- 특정 시간 이후 강한 적 등장
- 중간보스 이후 난이도 상승
```

우선순위:

```text
높음
```

이유:

```text
뱀서류 게임의 핵심은 시간이 지날수록 압박이 강해지는 체감이다.
현재는 기능은 있지만 반복감이 남을 수 있다.
```

---

### 6.3 무기 성장 구조 정리 필요

현재 근접 무기와 Magic Bolt가 존재한다.

문제:

```text
- Damage / AttackSpeed / AttackRange 강화가 두 무기에 동시에 적용됨
- 장기적으로 무기별 개별 강화와 공통 강화가 섞일 수 있음
- 무기 3개 이상이 되면 유지보수가 어려워질 수 있음
```

개선 방향:

```text
- 공통 강화와 개별 강화 기준 분리
- 새 무기 추가 전 WeaponUnlock 구조 점검
- Magic Bolt 전용 강화는 추후 별도 설계
```

우선순위:

```text
중간
```

이유:

```text
지금 당장 게임이 멈추는 문제는 아니다.
하지만 무기를 더 추가하기 전에 기준을 잡아야 한다.
```

---

### 6.4 UI 시각 피드백 부족

현재 UI는 기능 중심이다.

개선 방향:

```text
- 유물 획득 시 짧은 피드백 추가
- 레벨업 선택지 가독성 개선
- HUD 배치 정리
- HP / EXP / Wave / Relic 시각 구분 강화
- 버튼 폰트와 크기 정리
```

우선순위:

```text
중간
```

이유:

```text
기능은 작동하지만 플레이어가 성장과 보상을 체감하기에는 아직 약하다.
```

---

### 6.5 전투 타격감 부족

현재 전투는 작동하지만 체감이 약할 수 있다.

개선 방향:

```text
- 피격 이펙트 개선
- 사운드 추가
- 카메라 흔들림 추가
- 데미지 숫자 표시 검토
- Magic Bolt 명중 이펙트 추가
```

우선순위:

```text
중간
```

이유:

```text
뱀서류 게임은 반복 전투가 중심이기 때문에 타격감이 약하면 금방 단조로워진다.
```

---

### 6.6 10분 기준 밸런스 테스트 필요

현재 5분 MVP 기준 기능은 갖춰졌지만, 10분 플레이 기준 검증이 필요하다.

테스트 항목:

```text
- 5분까지 지루하지 않은지
- 5분 중간보스 난이도가 적절한지
- 5분 이후 난이도가 상승하는지
- 레벨업 속도가 적절한지
- 유물 효과가 체감되는지
- Magic Bolt 해금 후 전투가 달라지는지
- 10분까지 플레이가 가능한지
```

우선순위:

```text
높음
```

이유:

```text
기능이 있어도 실제 10분 플레이가 재미없으면 수직 슬라이스로 보기 어렵다.
```

---

## 7. 현재 우선순위 로드맵

---

### 1순위: 문서 최신화

목표:

```text
AGENTS.md
DevState.md
README.md
```

상태:

```text
진행 중
```

작업 이유:

```text
현재 기존 MD 문서가 실제 프로젝트 상태보다 뒤처졌다.
AI가 예전 문서를 기준으로 답하면 중복 시스템을 만들거나 이미 끝난 작업을 다시 제안할 수 있다.
```

---

### 2순위: PauseManager 정리

목표:

```text
Time.timeScale 직접 제어를 줄이고 PauseManager 중심으로 통합한다.
```

대상 파일 후보:

```text
Assets/_Project/Scripts/Core/PauseManager.cs
Assets/_Project/Scripts/Core/GameFlowManager.cs
Assets/_Project/Scripts/Player/PlayerHealth.cs
Assets/_Project/Scripts/UI/LevelUpUI.cs
Assets/_Project/Scripts/UI/RelicSelectUI.cs
```

완료 조건:

```text
- LevelUpUI에서 pause 정상
- RelicSelectUI에서 pause 정상
- GameOver에서 pause 정상
- Retry 후 Time.timeScale 정상 복구
- Console 에러 없음
```

---

### 3순위: 웨이브 체감 강화

목표:

```text
웨이브가 바뀔 때 실제 난이도 변화가 느껴지게 한다.
```

대상 파일 후보:

```text
Assets/_Project/Scripts/Spawning/WaveManager.cs
Assets/_Project/Scripts/Spawning/EnemySpawner.cs
```

개선 후보:

```text
- 웨이브별 maxEnemies 증가
- 웨이브별 spawnInterval 감소
- 웨이브별 적 조합 변화
- 5분 중간보스 이후 난이도 증가
```

완료 조건:

```text
- Wave Text 정상 갱신
- 웨이브가 오를수록 적 압박 증가
- 5분 중간보스 정상 등장
- 중간보스 이후 난이도 상승 체감
- Console 에러 없음
```

---

### 4순위: Magic Bolt 연결 점검

목표:

```text
Magic Bolt 해금, 발사, 투사체, 강화 적용 흐름을 검증한다.
```

대상 파일 후보:

```text
Assets/_Project/Scripts/Weapons/PlayerMagicBoltAutoAttack.cs
Assets/_Project/Scripts/Projectiles/MagicBoltProjectile.cs
Assets/_Project/Scripts/UI/LevelUpUI.cs
```

완료 조건:

```text
- Magic Bolt 해금 전에는 발사되지 않음
- Magic Bolt 해금 후 발사됨
- 투사체가 적을 향해 이동함
- 적에게 명중하면 데미지 적용
- 투사체가 정상 제거됨
- Console 에러 없음
```

---

### 5순위: 유물 획득 피드백 강화

목표:

```text
유물을 얻었을 때 플레이어가 확실히 인지할 수 있게 한다.
```

대상 파일 후보:

```text
Assets/_Project/Scripts/UI/RelicSelectUI.cs
Assets/_Project/Scripts/UI/HUDCanvasUI.cs
Assets/_Project/Scripts/Player/PlayerRelicEffects.cs
```

개선 후보:

```text
- 유물 획득 메시지
- 유물 획득 사운드
- HUD 유물 아이콘 표시
- 유물 효과 적용 시 피드백
```

완료 조건:

```text
- 유물 선택 후 HUD에 즉시 반영
- 선택한 유물 이름이 명확히 표시
- 유물 효과가 실제 적용됨
- Console 에러 없음
```

---

### 6순위: 10분 수직 슬라이스 확장

목표:

```text
10분 동안 지루하지 않고, 성장과 위협이 점진적으로 커지는 플레이 구조를 만든다.
```

추가 후보:

```text
- 새 적 1종
- 새 무기 1종
- 새 유물 2~3개
- 웨이브별 조합 변화
- 10분 보스 또는 강한 이벤트
```

완료 조건:

```text
- 10분까지 플레이 가능
- 5분 이후 체감 변화 존재
- 레벨업/무기/유물 선택이 의미 있음
- 반복감이 줄어듦
```

---

## 8. 현재 개발 시 주의할 점

---

### 8.1 새 UI 생성 금지

현재 UI 구조가 이미 존재한다.

```text
HUDCanvas
LevelUpCanvas
RelicSelectUI
StartMenuCanvas
GameOverCanvas
```

따라서 새 Canvas를 만들기보다 기존 UI를 수정한다.

금지 예:

```text
- HUDCanvas2 생성
- NewLevelUpUI 생성
- RelicCanvas 새로 생성
- GameOverManager 새로 생성
```

---

### 8.2 Debug UI와 정식 UI 구분

`HUDDebugUI.cs`는 개발 보조용이다.

출시형 HUD는 `HUDCanvasUI.cs` 기준으로 작업한다.

주의:

```text
- Debug UI를 정식 HUD처럼 확장하지 않는다.
- 정식 표시 기능은 HUDCanvasUI로 옮긴다.
```

---

### 8.3 ScriptableObject 구조 유지

강화와 유물은 ScriptableObject 기반이다.

```text
UpgradeData
RelicData
```

새 강화나 유물은 가능하면 ScriptableObject 에셋을 추가해서 관리한다.

주의:

```text
- 강화 데이터를 코드에 하드코딩하지 않는다.
- 유물 데이터를 코드에 계속 추가하지 않는다.
- 데이터는 ScriptableObject, 적용 로직은 관련 스크립트에서 관리한다.
```

---

### 8.4 .meta 파일 유지

Unity 프로젝트에서는 `.meta` 파일을 삭제하면 에셋 연결이 깨질 수 있다.

Git 커밋 시 `.meta` 파일은 같이 포함한다.

주의:

```text
- .meta 파일을 임의로 삭제하지 않는다.
- Prefab, ScriptableObject, Scene 수정 시 관련 .meta 파일도 함께 관리한다.
```

---

### 8.5 기존 시스템 중복 생성 금지

현재 이미 존재하는 핵심 시스템:

```text
- GameFlowManager
- GameTimer
- PauseManager
- HUDCanvasUI
- LevelUpUI
- RelicSelectUI
- EnemySpawner
- WaveManager
- PlayerHealth
- PlayerExp
- PlayerMeleeAutoAttack
- PlayerMagicBoltAutoAttack
```

AI는 위 기능을 대체하는 새 시스템을 임의로 만들지 않는다.

---

## 9. 다음 작업 전 체크리스트

새 작업을 시작하기 전 확인할 항목:

```text
- Unity Console 에러 없음
- GitHub 최신 커밋 완료
- 현재 씬 Main 열려 있음
- Play 버튼으로 정상 시작 가능
- Start 버튼 정상 작동
- Player 이동 가능
- 몬스터 스폰 가능
- 자동 공격 가능
- 경험치 획득 가능
- 레벨업 UI 정상
- 유물 선택 UI 정상
- GameOver / Retry 정상
```

---

## 10. 기능 수정 후 기본 테스트 기준

기능 수정 후 최소 확인 항목:

```text
1. Unity Console 에러 없음
2. Play 진입 가능
3. Start 버튼 작동
4. 플레이어 이동 가능
5. 몬스터 스폰 정상
6. 자동 공격 정상
7. 경험치 획득 정상
8. 레벨업 UI 정상
9. 유물 선택 UI 정상
10. 게임오버 / 재시작 정상
```

웨이브 관련 수정 후 추가 테스트:

```text
- Wave Text 갱신 확인
- Wave 변경 시 난이도 변화 확인
- 중간보스 스폰 확인
- 중간보스 처치 후 유물 선택 UI 확인
```

무기 관련 수정 후 추가 테스트:

```text
- 근접 공격 유지 확인
- Magic Bolt 해금 전/후 동작 확인
- 투사체가 적을 향해 발사되는지 확인
- 공격력/공속/범위 강화가 의도대로 적용되는지 확인
```

유물 관련 수정 후 추가 테스트:

```text
- 중간보스 처치 후 유물 선택 UI 표시
- 유물 3개 선택지 표시
- 유물 선택 후 HUD 반영
- 동일 유물 중복 선택 방지
- 유물 효과 적용 확인
```

Pause 관련 수정 후 추가 테스트:

```text
- LevelUpUI 표시 중 게임 정지
- RelicSelectUI 표시 중 게임 정지
- 선택 후 게임 정상 재개
- GameOver 시 게임 정지
- Retry 후 게임 정상 재시작
```

---

## 11. 다음 작업 추천

현재 바로 다음 작업은 다음 중 하나다.

```text
1. PauseManager 중심으로 timeScale 정리
2. WaveManager / EnemySpawner 기반 웨이브 체감 강화
3. Magic Bolt 연결 상태 점검
```

추천 순서:

```text
1. PauseManager 정리
2. 웨이브 체감 강화
3. Magic Bolt 점검
```

이유:

```text
- Pause 충돌은 UI가 늘어날수록 문제를 만든다.
- 웨이브 체감은 게임 재미에 직접 영향이 크다.
- Magic Bolt는 이미 들어왔으므로 다음 무기 추가 전 검증이 필요하다.
```

---

## 12. 단기 개발 로드맵

### Step 1. 문서 최신화

대상:

```text
AGENTS.md
Assets/_Project/Docs/DevState.md
README.md
```

목표:

```text
AI가 현재 프로젝트 상태를 잘못 이해하지 않도록 기준 문서를 최신화한다.
```

---

### Step 2. PauseManager 정리

목표:

```text
Time.timeScale 제어를 PauseManager 중심으로 통합한다.
```

---

### Step 3. 웨이브 체감 강화

목표:

```text
시간이 지날수록 적 압박이 강해지는 구조를 만든다.
```

---

### Step 4. Magic Bolt 점검

목표:

```text
Magic Bolt 해금과 전투 흐름을 안정화한다.
```

---

### Step 5. 유물 피드백 강화

목표:

```text
유물을 획득했을 때 보상감이 확실히 느껴지게 한다.
```

---

### Step 6. 10분 플레이 테스트

목표:

```text
10분 동안 플레이 가능한 수직 슬라이스 기준을 검증한다.
```

---

## 13. 중기 개발 로드맵

10분 수직 슬라이스로 확장하기 위한 후보 작업:

```text
- 새 적 1종 추가
- 새 무기 1종 추가
- 새 유물 2~3개 추가
- 10분 보스 또는 강한 이벤트 추가
- HUD 디자인 1차 정리
- 사운드 / 이펙트 1차 적용
- 타격감 개선
- 바닥 타일 / 배경 아트 교체
```

주의:

```text
한 번에 모두 진행하지 않는다.
항상 한 기능씩 추가하고 테스트한다.
```

---

## 14. 현재 결론

현재 프로젝트는 다음 상태다.

```text
5분 MVP 핵심 루프는 대부분 구현됨.
레벨업, 유물, HUD, 웨이브, 중간보스, Magic Bolt까지 들어온 상태다.
이제 무작정 기능을 늘리는 단계가 아니다.
```

앞으로 중요한 방향:

```text
1. 구조 충돌 제거
2. 웨이브 재미 강화
3. 성장 체감 강화
4. UI 피드백 강화
5. 10분 플레이 기준 밸런스 검증
```

현재 최우선 작업:

```text
문서 최신화 완료 후 PauseManager 정리로 넘어간다.
```