# EXPERT GPT / DEV SNAPSHOT

Generated: 2026-05-07 07:22:36
Unity Version: 6000.3.10f1
Active Scene: Main
Scene Path: Assets/_Project/Scenes/Main.unity
Project Root: Assets/_Project

## HOW GPT MUST USE THIS SNAPSHOT

1. 먼저 `Executive Summary`, `Diagnostics`, `Scene Hierarchy`, `UI Structure`, `Important Scene Objects`를 읽는다.
2. 새 스크립트나 새 GameObject를 제안하기 전에 기존 구조로 해결 가능한지 판단한다.
3. 코드 작성 전 반드시 수정 대상 파일과 이유를 먼저 말한다.
4. 사용자가 현재 구조를 묻는 경우 이 Snapshot 내용을 기준으로 답한다.
5. 이 Snapshot에 없는 오브젝트/스크립트/연결을 있다고 가정하지 않는다.

## HARD RULES

- 기존 HUD 구조 확인 전 새 HUD Canvas/Panel/Script 생성 금지.
- DebugUI와 정식 UI 혼동 금지.
- 새 기능 추가 전 기존 Manager/UI/Data 구조 재사용 우선.
- Inspector 연결이 필요한 경우 연결 대상 필드명과 오브젝트명을 반드시 명시.
- 전체 코드 제공 시 파일명/경로/전체 교체 여부를 반드시 명시.
- 오류가 있으면 추측하지 말고 Console 로그/파일/Hierarchy 기준으로 판단.
- 한 번에 여러 시스템을 동시에 수정하지 말 것.
- 새 파일 추가는 반드시 `왜 기존 파일 수정으로 안 되는지` 설명한 뒤 제안.

## Executive Summary

- Scene Objects: 61
- C# Scripts: 28
- Prefabs: 7
- Asset Files: 21
- Changed Files: Added 0, Modified 0, Deleted 0
- Diagnostics: HIGH 24, MEDIUM 4, LOW 14

### Key Existing Systems Detected

- HUD: OK (Script: HUDCanvasUI.cs=True, Object: HUDCanvas=True)
- Level Up: OK (Script: LevelUpUI.cs=True, Object: LevelUpCanvas=True)
- Relic Select: OK (Script: RelicSelectUI.cs=True, Object: RelicSelectUI=True)
- Relic Effects: OK (Script: PlayerRelicEffects.cs=True, Object: Player=True)
- Spawner: OK (Script: EnemySpawner.cs=True, Object: EnemySpawner=True)
- Wave: OK (Script: WaveManager.cs=True, Object: WaveManager=True)
- Timer: OK (Script: GameTimer.cs=True, Object: GameTimer=True)

## DevState.md

```markdown
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
```

## Diagnostics

### HIGH

1. **Possible Missing Inspector Reference**
   - Detail: RelicSelectUI / RelicSelectUI.relicButtonText01 = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
2. **Possible Missing Inspector Reference**
   - Detail: RelicSelectUI / RelicSelectUI.relicButtonText02 = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
3. **Possible Missing Inspector Reference**
   - Detail: RelicSelectUI / RelicSelectUI.relicButtonText03 = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
4. **Possible Missing Inspector Reference**
   - Detail: RelicSelectUI/RelicSelectPanel/TitleText / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
5. **Possible Missing Inspector Reference**
   - Detail: RelicSelectUI/RelicSelectPanel/InfoText / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
6. **Possible Missing Inspector Reference**
   - Detail: RelicSelectUI/RelicSelectPanel/RelicButton_01/Text (TMP) / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
7. **Possible Missing Inspector Reference**
   - Detail: RelicSelectUI/RelicSelectPanel/RelicButton_02/Text (TMP) / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
8. **Possible Missing Inspector Reference**
   - Detail: RelicSelectUI/RelicSelectPanel/RelicButton_03/Text (TMP) / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
9. **Possible Missing Inspector Reference**
   - Detail: StartMenuCanvas/StartPanel/StartButton/Text (TMP) / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
10. **Possible Missing Inspector Reference**
   - Detail: GameOverCanvas/GameOverPanel/Text (TMP) / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
11. **Possible Missing Inspector Reference**
   - Detail: GameOverCanvas/GameOverPanel/RetryButton/Text (TMP) / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
12. **Possible Missing Inspector Reference**
   - Detail: LevelUpCanvas / LevelUpUI.damageButtonText = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
13. **Possible Missing Inspector Reference**
   - Detail: LevelUpCanvas / LevelUpUI.attackSpeedButtonText = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
14. **Possible Missing Inspector Reference**
   - Detail: LevelUpCanvas / LevelUpUI.attackRangeButtonText = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
15. **Possible Missing Inspector Reference**
   - Detail: LevelUpCanvas/LevelUpPanel/TitleText / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
16. **Possible Missing Inspector Reference**
   - Detail: LevelUpCanvas/LevelUpPanel/PendingText / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
17. **Possible Missing Inspector Reference**
   - Detail: LevelUpCanvas/LevelUpPanel/DamageButton/Text (TMP) / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
18. **Possible Missing Inspector Reference**
   - Detail: LevelUpCanvas/LevelUpPanel/AttackSpeedButton/Text (TMP) / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
19. **Possible Missing Inspector Reference**
   - Detail: LevelUpCanvas/LevelUpPanel/AttackRangeButton/Text (TMP) / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
20. **Possible Missing Inspector Reference**
   - Detail: HUDCanvas/HUDPanel/HpText / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
21. **Possible Missing Inspector Reference**
   - Detail: HUDCanvas/HUDPanel/ExpText / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
22. **Possible Missing Inspector Reference**
   - Detail: HUDCanvas/HUDPanel/WaveText / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
23. **Possible Missing Inspector Reference**
   - Detail: HUDCanvas/HUDPanel/RelicText / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
24. **Possible Missing Inspector Reference**
   - Detail: HUDCanvas/TimerText / TextMeshProUGUI.parentLinkedComponent = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.

### MEDIUM

1. **HUD DebugUI and Official HUD UI Coexist**
   - Detail: HUDDebugUI.cs와 HUDCanvasUI.cs가 동시에 존재함.
   - Suggestion: 신규 HUD 작업은 HUDCanvasUI 기준으로 진행하고, HUDDebugUI는 디버그/보류 후보로 분류한다.
2. **Possible Missing Inspector Reference**
   - Detail: Player / PlayerMagicBoltAutoAttack.projectileSpawnPoint = None
   - Suggestion: 이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다.
3. **No Persistent Relic HUD Display**
   - Detail: 유물 선택 UI는 있으나 HUD에 보유 유물을 지속 표시하는 구조가 감지되지 않음.
   - Suggestion: 새 UI를 만들기 전 HUDCanvasUI와 기존 HUDPanel 구조를 먼저 확인하고 통합 방식을 결정한다.
4. **Drop Items May Overlap**
   - Detail: EnemyHealth에서 ExpGem과 SmallHeal이 같은 transform.position으로 생성될 가능성이 있음.
   - Suggestion: SmallHeal 드랍 위치를 약간 오프셋하여 ExpGem과 겹치지 않게 한다.

### LOW

1. **OnGUI Usage Detected**
   - Detail: Assets/_Project/Scripts/Debug/HUDDebugUI.cs에서 OnGUI 사용 감지.
   - Suggestion: OnGUI는 테스트/디버그용으로만 유지하고 출시형 UI는 Canvas 기반으로 진행한다.
2. **OnGUI Usage Detected**
   - Detail: Assets/_Project/Scripts/Editor/DevSnapshotTool.cs에서 OnGUI 사용 감지.
   - Suggestion: OnGUI는 테스트/디버그용으로만 유지하고 출시형 UI는 Canvas 기반으로 진행한다.
3. **FindFirstObjectByType Usage**
   - Detail: Assets/_Project/Scripts/Core/GameFlowManager.cs에서 FindFirstObjectByType 사용 감지.
   - Suggestion: 초기 단계에서는 허용 가능. 호출 빈도가 높거나 확장되면 Inspector 참조 또는 이벤트 구조로 교체 검토.
4. **Direct TimeScale Control**
   - Detail: Assets/_Project/Scripts/Core/GameFlowManager.cs에서 Time.timeScale 직접 제어 감지.
   - Suggestion: 여러 UI가 동시에 pause를 제어하면 충돌 가능. 나중에 PauseManager로 통합 검토.
5. **FindFirstObjectByType Usage**
   - Detail: Assets/_Project/Scripts/Editor/DevSnapshotTool.cs에서 FindFirstObjectByType 사용 감지.
   - Suggestion: 초기 단계에서는 허용 가능. 호출 빈도가 높거나 확장되면 Inspector 참조 또는 이벤트 구조로 교체 검토.
6. **Direct TimeScale Control**
   - Detail: Assets/_Project/Scripts/Editor/DevSnapshotTool.cs에서 Time.timeScale 직접 제어 감지.
   - Suggestion: 여러 UI가 동시에 pause를 제어하면 충돌 가능. 나중에 PauseManager로 통합 검토.
7. **FindFirstObjectByType Usage**
   - Detail: Assets/_Project/Scripts/Enemy/EnemyHealth.cs에서 FindFirstObjectByType 사용 감지.
   - Suggestion: 초기 단계에서는 허용 가능. 호출 빈도가 높거나 확장되면 Inspector 참조 또는 이벤트 구조로 교체 검토.
8. **Destroy Enemy Object**
   - Detail: Assets/_Project/Scripts/Enemy/EnemyHealth.cs에서 Destroy(gameObject) 사용.
   - Suggestion: 5분 MVP에서는 허용. 적 수가 많아지면 Object Pooling 검토.
9. **FindFirstObjectByType Usage**
   - Detail: Assets/_Project/Scripts/Spawning/WaveManager.cs에서 FindFirstObjectByType 사용 감지.
   - Suggestion: 초기 단계에서는 허용 가능. 호출 빈도가 높거나 확장되면 Inspector 참조 또는 이벤트 구조로 교체 검토.
10. **FindFirstObjectByType Usage**
   - Detail: Assets/_Project/Scripts/UI/HUDCanvasUI.cs에서 FindFirstObjectByType 사용 감지.
   - Suggestion: 초기 단계에서는 허용 가능. 호출 빈도가 높거나 확장되면 Inspector 참조 또는 이벤트 구조로 교체 검토.
11. **FindFirstObjectByType Usage**
   - Detail: Assets/_Project/Scripts/UI/LevelUpUI.cs에서 FindFirstObjectByType 사용 감지.
   - Suggestion: 초기 단계에서는 허용 가능. 호출 빈도가 높거나 확장되면 Inspector 참조 또는 이벤트 구조로 교체 검토.
12. **Direct TimeScale Control**
   - Detail: Assets/_Project/Scripts/UI/LevelUpUI.cs에서 Time.timeScale 직접 제어 감지.
   - Suggestion: 여러 UI가 동시에 pause를 제어하면 충돌 가능. 나중에 PauseManager로 통합 검토.
13. **FindFirstObjectByType Usage**
   - Detail: Assets/_Project/Scripts/UI/RelicSelectUI.cs에서 FindFirstObjectByType 사용 감지.
   - Suggestion: 초기 단계에서는 허용 가능. 호출 빈도가 높거나 확장되면 Inspector 참조 또는 이벤트 구조로 교체 검토.
14. **Direct TimeScale Control**
   - Detail: Assets/_Project/Scripts/UI/RelicSelectUI.cs에서 Time.timeScale 직접 제어 감지.
   - Suggestion: 여러 UI가 동시에 pause를 제어하면 충돌 가능. 나중에 PauseManager로 통합 검토.

## Recommended Next Steps

1. HIGH 진단 항목이 있으므로 새 기능 추가 전에 연결 누락/중복/누락 스크립트부터 정리한다.
2. 유물 보유 표시를 추가할 때는 새 HUD Canvas부터 만들지 말고, 기존 HUDCanvas/HUDPanel/HUDCanvasUI 구조를 먼저 검토한 뒤 통합 여부를 결정한다.
3. SmallHeal과 ExpGem이 같은 위치에 드랍될 가능성이 높다. 다음 작업은 EnemyHealth 드랍 위치 오프셋 분리로 잡는 것이 안전하다.

## Existing Systems To Reuse

### UI
- HUD 작업: HUDCanvas, HUDPanel, HUDCanvasUI 우선 확인.
- 레벨업 작업: LevelUpCanvas, LevelUpUI, UpgradeData 우선 활용.
- 유물 선택 작업: RelicSelectUI, RelicData, PlayerRelicEffects 우선 활용.
- DebugUI 파일은 신규 기능의 기준으로 삼지 말 것.

### Gameplay
- 플레이어 체력/방어/회복: PlayerHealth.
- 플레이어 이동/넉백: PlayerController.
- 근접 자동공격/치명타: PlayerMeleeAutoAttack.
- 경험치/레벨업: PlayerExp + LevelUpUI.
- 적 체력/드랍/중간보스 사망 처리: EnemyHealth.
- 적 스폰/중간보스 스폰: EnemySpawner + WaveManager.

### Data
- 강화 데이터: UpgradeData ScriptableObject.
- 유물 데이터: RelicData ScriptableObject.

## Current Scene Hierarchy

```text
- Main Camera [Transform, Camera, AudioListener, UniversalAdditionalCameraData, CameraFollow2D]
- Global Light 2D [Transform, Light2D]
- Player [Transform, SpriteRenderer, Rigidbody2D, BoxCollider2D, PlayerController, PlayerMeleeAutoAttack, PlayerHealth, PlayerExp, PlayerPickupRange, PlayerRelicEffects, PlayerMagicBoltAutoAttack]
  - MeleeSlashVisual (inactive) [Transform, SpriteRenderer]
- EnemySpawner [Transform, EnemySpawner]
- WaveManager [Transform, WaveManager]
- RelicSelectUI [RectTransform, Canvas, CanvasScaler, GraphicRaycaster, RelicSelectUI]
  - RelicSelectPanel [RectTransform, CanvasRenderer, Image]
    - TitleText [RectTransform, CanvasRenderer, TextMeshProUGUI]
    - InfoText [RectTransform, CanvasRenderer, TextMeshProUGUI]
    - RelicButton_01 [RectTransform, CanvasRenderer, Image, Button]
      - Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
    - RelicButton_02 [RectTransform, CanvasRenderer, Image, Button]
      - Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
    - RelicButton_03 [RectTransform, CanvasRenderer, Image, Button]
      - Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
- EventSystem [Transform, EventSystem, InputSystemUIInputModule]
- StageTileGenerator [Transform, StageTileGenerator]
- GroundGrid [Transform, Grid]
  - GroundTilemap [Transform, Tilemap, TilemapRenderer]
- GameFlowManager [Transform, GameFlowManager]
- StartMenuCanvas [RectTransform, Canvas, CanvasScaler, GraphicRaycaster]
  - StartPanel [RectTransform, CanvasRenderer, Image]
    - StartButton [RectTransform, CanvasRenderer, Image, Button]
      - Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
- GameOverCanvas (inactive) [RectTransform, Canvas, CanvasScaler, GraphicRaycaster]
  - GameOverPanel [RectTransform, CanvasRenderer, Image]
    - Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
    - RetryButton [RectTransform, CanvasRenderer, Image, Button]
      - Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
- LevelUpCanvas [RectTransform, Canvas, CanvasScaler, GraphicRaycaster, LevelUpUI]
  - LevelUpPanel [RectTransform, CanvasRenderer, Image]
    - TitleText [RectTransform, CanvasRenderer, TextMeshProUGUI]
    - PendingText [RectTransform, CanvasRenderer, TextMeshProUGUI]
    - DamageButton [RectTransform, CanvasRenderer, Image, Button]
      - Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
    - AttackSpeedButton [RectTransform, CanvasRenderer, Image, Button]
      - Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
    - AttackRangeButton [RectTransform, CanvasRenderer, Image, Button]
      - Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
- GameTimer [Transform, GameTimer]
- HUDCanvas [RectTransform, Canvas, CanvasScaler, GraphicRaycaster, HUDCanvasUI]
  - HUDPanel [RectTransform, CanvasRenderer, Image]
    - HpSlider [RectTransform, Slider]
      - Background [RectTransform, CanvasRenderer, Image]
      - Fill Area [RectTransform]
        - Fill [RectTransform, CanvasRenderer, Image]
      - Handle Slide Area (inactive) [RectTransform]
        - Handle [RectTransform, CanvasRenderer, Image]
    - HpText [RectTransform, CanvasRenderer, TextMeshProUGUI]
    - ExpSlider [RectTransform, Slider]
      - Background [RectTransform, CanvasRenderer, Image]
      - Fill Area [RectTransform]
        - Fill [RectTransform, CanvasRenderer, Image]
      - Handle Slide Area (inactive) [RectTransform]
        - Handle [RectTransform, CanvasRenderer, Image]
    - ExpText [RectTransform, CanvasRenderer, TextMeshProUGUI]
    - WaveText [RectTransform, CanvasRenderer, TextMeshProUGUI]
    - RelicText [RectTransform, CanvasRenderer, TextMeshProUGUI]
  - TimerText [RectTransform, CanvasRenderer, TextMeshProUGUI]
- PauseManager [Transform, PauseManager]
```

## Important Scene Objects / Inspector Snapshot

### Player
- Active: True
- Tag: Untagged
- Layer: Default
- Components: Transform, SpriteRenderer, Rigidbody2D, BoxCollider2D, PlayerController, PlayerMeleeAutoAttack, PlayerHealth, PlayerExp, PlayerPickupRange, PlayerRelicEffects, PlayerMagicBoltAutoAttack

#### Transform
```text
Local Position: (0.00, -0.02, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (0.80, 0.80, 1.00)
```

#### SpriteRenderer
```text
No visible serialized fields
```

#### Rigidbody2D
```text
No visible serialized fields
```

#### BoxCollider2D
```text
No visible serialized fields
```

#### PlayerController
```text
moveSpeed: 5
maximumMoveSpeed: 9
```

#### PlayerMeleeAutoAttack
```text
damage: 10
attackInterval: 0.8
attackRange: 1.15
attackRadius: 0.55
enemyLayer: 64
criticalChance: 0
criticalMultiplier: 2
maximumCriticalChance: 0.5
relicAttackSpeedBonus: 0
minimumAttackInterval: 0.2
maximumAttackRange: 3.5
maximumAttackRadius: 2
attackVisual: MeleeSlashVisual (GameObject)
visualDuration: 0.12
```

#### PlayerHealth
```text
maxHealth: 100
defense: 0
maximumDefense: 20
invincibleDuration: 0.6
knockbackForce: 5
knockbackDuration: 0.15
hitColor: RGBA(1.000, 0.000, 0.000, 1.000)
blinkInterval: 0.08
```

#### PlayerExp
```text
level: 1
currentExp: 0
expToNextLevel: 20
levelUpUI: LevelUpCanvas (LevelUpUI)
```

#### PlayerPickupRange
```text
pickupRadius: 1.5
maximumPickupRadius: 6
scanInterval: 0.1
targetLayers: -1
```

#### PlayerRelicEffects
```text
playerHealth: Player (PlayerHealth)
playerWeapon: Player (PlayerMeleeAutoAttack)
hasBerserkerFang: False
berserkerAttackSpeedBonus: 0.25
berserkerHpThreshold: 0.4
hasBloodSigil: False
bloodSigilHealChance: 0.05
bloodSigilHealAmount: 1
```

#### PlayerMagicBoltAutoAttack
```text
isUnlocked: False
projectilePrefab: MagicBolt (GameObject)
projectileSpawnPoint: None
useProjectilePooling: True
projectilePoolPrewarmCount: 12
damage: 8
attackInterval: 1.2
attackRange: 6
enemyLayer: 64
minimumAttackInterval: 0.25
maximumAttackRange: 10
```

### Player/MeleeSlashVisual
- Active: False
- Tag: Untagged
- Layer: Default
- Components: Transform, SpriteRenderer

#### Transform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.40, 0.45, 1.00)
```

#### SpriteRenderer
```text
No visible serialized fields
```

### EnemySpawner
- Active: True
- Tag: Untagged
- Layer: Default
- Components: Transform, EnemySpawner

#### Transform
```text
Local Position: (1.55, -0.04, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### EnemySpawner
```text
playerTarget: Player (Transform)
waveManager: WaveManager (WaveManager)
enemySpawnEntries: Array/List Size: 2
spawnInterval: 2
spawnRadius: 10
maxEnemies: 10
useEnemyPooling: True
prewarmPerEnemyType: 6
```

### WaveManager
- Active: True
- Tag: Untagged
- Layer: Default
- Components: Transform, WaveManager

#### Transform
```text
Local Position: (1.55, -0.04, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### WaveManager
```text
gameTimer: GameTimer (GameTimer)
currentWave: 1
waveDuration: 30
baseEnemyHealth: 30
healthPerWave: 5
baseEnemyDamage: 8
damagePerWave: 1
baseExpReward: 5
firstMidBossSpawnTime: 300
midBossHealthMultiplier: 3
midBossDamageMultiplier: 1.5
midBossExpMultiplier: 4
```

### RelicSelectUI
- Active: True
- Tag: Untagged
- Layer: Default
- Components: RectTransform, Canvas, CanvasScaler, GraphicRaycaster, RelicSelectUI

#### RectTransform
```text
Local Position: (573.00, 256.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### Canvas
```text
No visible serialized fields
```

#### CanvasScaler
```text
No visible serialized fields
```

#### GraphicRaycaster
```text
No visible serialized fields
```

#### RelicSelectUI
```text
playerHealth: Player (PlayerHealth)
pauseManager: PauseManager (PauseManager)
playerWeapon: Player (PlayerMeleeAutoAttack)
playerPickupRange: Player (PlayerPickupRange)
playerRelicEffects: Player (PlayerRelicEffects)
availableRelics: Array/List Size: 5
excludeOwnedRelics: True
relicSelectPanel: RelicSelectPanel (GameObject)
titleText: TitleText (TextMeshProUGUI)
infoText: InfoText (TextMeshProUGUI)
relicButton01: RelicButton_01 (Button)
relicButton02: RelicButton_02 (Button)
relicButton03: RelicButton_03 (Button)
relicButtonText01: None
relicButtonText02: None
relicButtonText03: None
```

### RelicSelectUI/RelicSelectPanel
- Active: True
- Tag: Untagged
- Layer: Default
- Components: RectTransform, CanvasRenderer, Image

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

### RelicSelectUI/RelicSelectPanel/TitleText
- Active: True
- Tag: Untagged
- Layer: Default
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 180.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### RelicSelectUI/RelicSelectPanel/InfoText
- Active: True
- Tag: Untagged
- Layer: Default
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 120.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### RelicSelectUI/RelicSelectPanel/RelicButton_01
- Active: True
- Tag: Untagged
- Layer: Default
- Components: RectTransform, CanvasRenderer, Image, Button

#### RectTransform
```text
Local Position: (-300.00, -40.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

#### Button
```text
No visible serialized fields
```

### RelicSelectUI/RelicSelectPanel/RelicButton_01/Text (TMP)
- Active: True
- Tag: Untagged
- Layer: Default
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### RelicSelectUI/RelicSelectPanel/RelicButton_02
- Active: True
- Tag: Untagged
- Layer: Default
- Components: RectTransform, CanvasRenderer, Image, Button

#### RectTransform
```text
Local Position: (0.00, -40.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

#### Button
```text
No visible serialized fields
```

### RelicSelectUI/RelicSelectPanel/RelicButton_02/Text (TMP)
- Active: True
- Tag: Untagged
- Layer: Default
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### RelicSelectUI/RelicSelectPanel/RelicButton_03
- Active: True
- Tag: Untagged
- Layer: Default
- Components: RectTransform, CanvasRenderer, Image, Button

#### RectTransform
```text
Local Position: (300.00, -40.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

#### Button
```text
No visible serialized fields
```

### RelicSelectUI/RelicSelectPanel/RelicButton_03/Text (TMP)
- Active: True
- Tag: Untagged
- Layer: Default
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### EventSystem
- Active: True
- Tag: Untagged
- Layer: Default
- Components: Transform, EventSystem, InputSystemUIInputModule

#### Transform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### EventSystem
```text
No visible serialized fields
```

#### InputSystemUIInputModule
```text
No visible serialized fields
```

### StageTileGenerator
- Active: True
- Tag: Untagged
- Layer: Default
- Components: Transform, StageTileGenerator

#### Transform
```text
Local Position: (1.55, -0.04, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### StageTileGenerator
```text
player: Player (Transform)
groundTilemap: GroundTilemap (Tilemap)
groundDirt01: Ground_Dirt_01 (Tile)
groundDirt02: Ground_Dirt_02 (Tile)
groundGrass01: Ground_Grass_01 (Tile)
groundGrass02: Ground_Grass_02 (Tile)
groundStone01: Ground_Stone_01 (Tile)
groundCrack01: Ground_Crack_01 (Tile)
chunkSize: 16
activeChunkRadius: 2
seed: 12345
clearExistingTilesOnStart: True
```

### GroundGrid
- Active: True
- Tag: Untagged
- Layer: Default
- Components: Transform, Grid

#### Transform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### Grid
```text
No visible serialized fields
```

### GroundGrid/GroundTilemap
- Active: True
- Tag: Untagged
- Layer: Default
- Components: Transform, Tilemap, TilemapRenderer

#### Transform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### Tilemap
```text
No visible serialized fields
```

#### TilemapRenderer
```text
No visible serialized fields
```

### GameFlowManager
- Active: True
- Tag: Untagged
- Layer: Default
- Components: Transform, GameFlowManager

#### Transform
```text
Local Position: (0.05, 0.11, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### GameFlowManager
```text
playerHealth: Player (PlayerHealth)
gameTimer: GameTimer (GameTimer)
pauseManager: PauseManager (PauseManager)
startMenuCanvas: StartMenuCanvas (GameObject)
gameOverCanvas: GameOverCanvas (GameObject)
hudCanvas: HUDCanvas (GameObject)
showStartMenuOnLoad: True
```

### StartMenuCanvas
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, Canvas, CanvasScaler, GraphicRaycaster

#### RectTransform
```text
Local Position: (573.00, 256.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (0.53, 0.53, 0.53)
```

#### Canvas
```text
No visible serialized fields
```

#### CanvasScaler
```text
No visible serialized fields
```

#### GraphicRaycaster
```text
No visible serialized fields
```

### StartMenuCanvas/StartPanel
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

### StartMenuCanvas/StartPanel/StartButton
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image, Button

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

#### Button
```text
No visible serialized fields
```

### StartMenuCanvas/StartPanel/StartButton/Text (TMP)
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### GameOverCanvas
- Active: False
- Tag: Untagged
- Layer: UI
- Components: RectTransform, Canvas, CanvasScaler, GraphicRaycaster

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (0.00, 0.00, 0.00)
```

#### Canvas
```text
No visible serialized fields
```

#### CanvasScaler
```text
No visible serialized fields
```

#### GraphicRaycaster
```text
No visible serialized fields
```

### GameOverCanvas/GameOverPanel
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

### GameOverCanvas/GameOverPanel/Text (TMP)
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 80.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### GameOverCanvas/GameOverPanel/RetryButton
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image, Button

#### RectTransform
```text
Local Position: (0.00, -40.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

#### Button
```text
No visible serialized fields
```

### GameOverCanvas/GameOverPanel/RetryButton/Text (TMP)
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### LevelUpCanvas
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, Canvas, CanvasScaler, GraphicRaycaster, LevelUpUI

#### RectTransform
```text
Local Position: (573.00, 256.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### Canvas
```text
No visible serialized fields
```

#### CanvasScaler
```text
No visible serialized fields
```

#### GraphicRaycaster
```text
No visible serialized fields
```

#### LevelUpUI
```text
playerHealth: Player (PlayerHealth)
pauseManager: PauseManager (PauseManager)
playerWeapon: Player (PlayerMeleeAutoAttack)
playerController: Player (PlayerController)
playerPickupRange: Player (PlayerPickupRange)
playerMagicBolt: Player (PlayerMagicBoltAutoAttack)
availableUpgrades: Array/List Size: 9
levelUpPanel: LevelUpPanel (GameObject)
titleText: TitleText (TextMeshProUGUI)
pendingText: PendingText (TextMeshProUGUI)
damageButton: DamageButton (Button)
attackSpeedButton: AttackSpeedButton (Button)
attackRangeButton: AttackRangeButton (Button)
damageButtonText: None
attackSpeedButtonText: None
attackRangeButtonText: None
```

### LevelUpCanvas/LevelUpPanel
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

### LevelUpCanvas/LevelUpPanel/TitleText
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 150.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### LevelUpCanvas/LevelUpPanel/PendingText
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 95.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### LevelUpCanvas/LevelUpPanel/DamageButton
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image, Button

#### RectTransform
```text
Local Position: (0.00, 35.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

#### Button
```text
No visible serialized fields
```

### LevelUpCanvas/LevelUpPanel/DamageButton/Text (TMP)
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### LevelUpCanvas/LevelUpPanel/AttackSpeedButton
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image, Button

#### RectTransform
```text
Local Position: (0.00, -45.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

#### Button
```text
No visible serialized fields
```

### LevelUpCanvas/LevelUpPanel/AttackSpeedButton/Text (TMP)
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### LevelUpCanvas/LevelUpPanel/AttackRangeButton
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image, Button

#### RectTransform
```text
Local Position: (0.00, -125.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

#### Button
```text
No visible serialized fields
```

### LevelUpCanvas/LevelUpPanel/AttackRangeButton/Text (TMP)
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### GameTimer
- Active: True
- Tag: Untagged
- Layer: Default
- Components: Transform, GameTimer

#### Transform
```text
Local Position: (0.05, 0.11, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### GameTimer
```text
isRunning: False
gameplayTime: 0
realTime: 0
logTimer: 0
timerText: TimerText (TextMeshProUGUI)
showDebugLog: False
logInterval: 10
```

### HUDCanvas
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, Canvas, CanvasScaler, GraphicRaycaster, HUDCanvasUI

#### RectTransform
```text
Local Position: (573.00, 256.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (0.53, 0.53, 0.53)
```

#### Canvas
```text
No visible serialized fields
```

#### CanvasScaler
```text
No visible serialized fields
```

#### GraphicRaycaster
```text
No visible serialized fields
```

#### HUDCanvasUI
```text
playerHealth: Player (PlayerHealth)
playerExp: Player (PlayerExp)
waveManager: WaveManager (WaveManager)
relicSelectUI: RelicSelectUI (RelicSelectUI)
hpSlider: HpSlider (Slider)
hpText: HpText (TextMeshProUGUI)
expSlider: ExpSlider (Slider)
expText: ExpText (TextMeshProUGUI)
waveText: WaveText (TextMeshProUGUI)
relicText: RelicText (TextMeshProUGUI)
relicTitle: RELICS
emptyRelicText: ""
waveUiRefreshInterval: 0.2
```

### HUDCanvas/HUDPanel
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

### HUDCanvas/HUDPanel/HpSlider
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, Slider

#### RectTransform
```text
Local Position: (-777.18, 426.25, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### Slider
```text
No visible serialized fields
```

### HUDCanvas/HUDPanel/HpSlider/Background
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

### HUDCanvas/HUDPanel/HpSlider/Fill Area
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

### HUDCanvas/HUDPanel/HpSlider/Fill Area/Fill
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

### HUDCanvas/HUDPanel/HpSlider/Handle Slide Area
- Active: False
- Tag: Untagged
- Layer: UI
- Components: RectTransform

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

### HUDCanvas/HUDPanel/HpSlider/Handle Slide Area/Handle
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image

#### RectTransform
```text
Local Position: (270.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

### HUDCanvas/HUDPanel/HpText
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (-777.18, 426.25, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### HUDCanvas/HUDPanel/ExpSlider
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, Slider

#### RectTransform
```text
Local Position: (-777.18, 371.25, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### Slider
```text
No visible serialized fields
```

### HUDCanvas/HUDPanel/ExpSlider/Background
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

### HUDCanvas/HUDPanel/ExpSlider/Fill Area
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

### HUDCanvas/HUDPanel/ExpSlider/Fill Area/Fill
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image

#### RectTransform
```text
Local Position: (-280.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

### HUDCanvas/HUDPanel/ExpSlider/Handle Slide Area
- Active: False
- Tag: Untagged
- Layer: UI
- Components: RectTransform

#### RectTransform
```text
Local Position: (0.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

### HUDCanvas/HUDPanel/ExpSlider/Handle Slide Area/Handle
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, Image

#### RectTransform
```text
Local Position: (-270.00, 0.00, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### Image
```text
No visible serialized fields
```

### HUDCanvas/HUDPanel/ExpText
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (-777.18, 371.25, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### HUDCanvas/HUDPanel/WaveText
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (-777.18, 321.25, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### HUDCanvas/HUDPanel/RelicText
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (-1897.18, 711.25, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### HUDCanvas/TimerText
- Active: True
- Tag: Untagged
- Layer: UI
- Components: RectTransform, CanvasRenderer, TextMeshProUGUI

#### RectTransform
```text
Local Position: (0.00, 441.25, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### CanvasRenderer
```text
No visible serialized fields
```

#### TextMeshProUGUI
```text
parentLinkedComponent: None
checkPaddingRequired: False
```

### PauseManager
- Active: True
- Tag: Untagged
- Layer: Default
- Components: Transform, PauseManager

#### Transform
```text
Local Position: (-0.95, 1.23, 0.00)
Local Rotation: (0.00, 0.00, 0.00)
Local Scale: (1.00, 1.00, 1.00)
```

#### PauseManager
```text
No visible serialized fields
```

## UI Structure Analysis

```text
- GameOverCanvas [RectTransform, Canvas, CanvasScaler, GraphicRaycaster]
- GameOverCanvas/GameOverPanel [RectTransform, CanvasRenderer, Image]
- GameOverCanvas/GameOverPanel/RetryButton [RectTransform, CanvasRenderer, Image, Button]
- GameOverCanvas/GameOverPanel/RetryButton/Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
- GameOverCanvas/GameOverPanel/Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
- HUDCanvas [RectTransform, Canvas, CanvasScaler, GraphicRaycaster, HUDCanvasUI]
- HUDCanvas/HUDPanel [RectTransform, CanvasRenderer, Image]
- HUDCanvas/HUDPanel/ExpSlider [RectTransform, Slider]
- HUDCanvas/HUDPanel/ExpSlider/Background [RectTransform, CanvasRenderer, Image]
- HUDCanvas/HUDPanel/ExpSlider/Fill Area/Fill [RectTransform, CanvasRenderer, Image]
- HUDCanvas/HUDPanel/ExpSlider/Handle Slide Area/Handle [RectTransform, CanvasRenderer, Image]
- HUDCanvas/HUDPanel/ExpText [RectTransform, CanvasRenderer, TextMeshProUGUI]
- HUDCanvas/HUDPanel/HpSlider [RectTransform, Slider]
- HUDCanvas/HUDPanel/HpSlider/Background [RectTransform, CanvasRenderer, Image]
- HUDCanvas/HUDPanel/HpSlider/Fill Area/Fill [RectTransform, CanvasRenderer, Image]
- HUDCanvas/HUDPanel/HpSlider/Handle Slide Area/Handle [RectTransform, CanvasRenderer, Image]
- HUDCanvas/HUDPanel/HpText [RectTransform, CanvasRenderer, TextMeshProUGUI]
- HUDCanvas/HUDPanel/RelicText [RectTransform, CanvasRenderer, TextMeshProUGUI]
- HUDCanvas/HUDPanel/WaveText [RectTransform, CanvasRenderer, TextMeshProUGUI]
- HUDCanvas/TimerText [RectTransform, CanvasRenderer, TextMeshProUGUI]
- LevelUpCanvas [RectTransform, Canvas, CanvasScaler, GraphicRaycaster, LevelUpUI]
- LevelUpCanvas/LevelUpPanel [RectTransform, CanvasRenderer, Image]
- LevelUpCanvas/LevelUpPanel/AttackRangeButton [RectTransform, CanvasRenderer, Image, Button]
- LevelUpCanvas/LevelUpPanel/AttackRangeButton/Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
- LevelUpCanvas/LevelUpPanel/AttackSpeedButton [RectTransform, CanvasRenderer, Image, Button]
- LevelUpCanvas/LevelUpPanel/AttackSpeedButton/Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
- LevelUpCanvas/LevelUpPanel/DamageButton [RectTransform, CanvasRenderer, Image, Button]
- LevelUpCanvas/LevelUpPanel/DamageButton/Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
- LevelUpCanvas/LevelUpPanel/PendingText [RectTransform, CanvasRenderer, TextMeshProUGUI]
- LevelUpCanvas/LevelUpPanel/TitleText [RectTransform, CanvasRenderer, TextMeshProUGUI]
- RelicSelectUI [RectTransform, Canvas, CanvasScaler, GraphicRaycaster, RelicSelectUI]
- RelicSelectUI/RelicSelectPanel [RectTransform, CanvasRenderer, Image]
- RelicSelectUI/RelicSelectPanel/InfoText [RectTransform, CanvasRenderer, TextMeshProUGUI]
- RelicSelectUI/RelicSelectPanel/RelicButton_01 [RectTransform, CanvasRenderer, Image, Button]
- RelicSelectUI/RelicSelectPanel/RelicButton_01/Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
- RelicSelectUI/RelicSelectPanel/RelicButton_02 [RectTransform, CanvasRenderer, Image, Button]
- RelicSelectUI/RelicSelectPanel/RelicButton_02/Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
- RelicSelectUI/RelicSelectPanel/RelicButton_03 [RectTransform, CanvasRenderer, Image, Button]
- RelicSelectUI/RelicSelectPanel/RelicButton_03/Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
- RelicSelectUI/RelicSelectPanel/TitleText [RectTransform, CanvasRenderer, TextMeshProUGUI]
- StartMenuCanvas [RectTransform, Canvas, CanvasScaler, GraphicRaycaster]
- StartMenuCanvas/StartPanel [RectTransform, CanvasRenderer, Image]
- StartMenuCanvas/StartPanel/StartButton [RectTransform, CanvasRenderer, Image, Button]
- StartMenuCanvas/StartPanel/StartButton/Text (TMP) [RectTransform, CanvasRenderer, TextMeshProUGUI]
```

### UI Warnings

- HUDCanvas exists: True
- HUDPanel exists: True
- TimerText exists: True
- RelicSelectPanel exists: True

- HUD 관련 신규 작업은 기존 HUDCanvas/HUDPanel 구조를 우선 검토해야 함.
- 유물 선택 UI는 Canvas 기반 구조가 이미 존재함. OnGUI DebugUI 기준으로 작업하지 말 것.

## ScriptableObjects Snapshot

### Assets/_Project/ScriptableObjects/Relics/Relic_BerserkerFang.asset
- Type: RelicData
```text
relicName: Berserker Fang
description: Low HP increases attack speed.
icon: None
relicType: Berserker Fang
intValue: 0
floatValue: 0.25
secondaryFloatValue: 0.4
canStack: False
maxStack: 1
```

### Assets/_Project/ScriptableObjects/Relics/Relic_BloodSigil.asset
- Type: RelicData
```text
relicName: Blood Sigil
description: Chance to heal when killing enemies.
icon: None
relicType: Blood Sigil
intValue: 1
floatValue: 0.5
secondaryFloatValue: 0
canStack: False
maxStack: 1
```

### Assets/_Project/ScriptableObjects/Relics/Relic_GraveMagnet.asset
- Type: RelicData
```text
relicName: Grave Magnet
description: Improves pickup range.
icon: None
relicType: Grave Magnet
intValue: 0
floatValue: 0.5
secondaryFloatValue: 0
canStack: False
maxStack: 1
```

### Assets/_Project/ScriptableObjects/Relics/Relic_HunterEye.asset
- Type: RelicData
```text
relicName: Hunter Eye
description: Increases critical power.
icon: None
relicType: Hunter Eye
intValue: 0
floatValue: 0.1
secondaryFloatValue: 0.25
canStack: False
maxStack: 1
```

### Assets/_Project/ScriptableObjects/Relics/Relic_IronCharm.asset
- Type: RelicData
```text
relicName: Iron Charm
description: Reduces damage taken.
icon: None
relicType: Iron Charm
intValue: 1
floatValue: 0
secondaryFloatValue: 0
canStack: False
maxStack: 1
```

### Assets/_Project/ScriptableObjects/Upgrades/Upgrade_AttackRange.asset
- Type: UpgradeData
```text
upgradeName: ATTACK RANGE
description: Range +12%
icon: None
upgradeType: Attack Range
intValue: 0
floatValue: 0.12
weaponId: ""
```

### Assets/_Project/ScriptableObjects/Upgrades/Upgrade_AttackSpeed.asset
- Type: UpgradeData
```text
upgradeName: ATTACK SPEED
description: Cooldown -15%
icon: None
upgradeType: Attack Speed
intValue: 0
floatValue: 0.15
weaponId: ""
```

### Assets/_Project/ScriptableObjects/Upgrades/Upgrade_CriticalChance.asset
- Type: UpgradeData
```text
upgradeName: CRITICAL CHANCE UP
description: Critical Chance +5%
icon: None
upgradeType: Critical Chance
intValue: 0
floatValue: 0.05
weaponId: ""
```

### Assets/_Project/ScriptableObjects/Upgrades/Upgrade_Damage.asset
- Type: UpgradeData
```text
upgradeName: DAMAGE UP
description: Damage +5
icon: None
upgradeType: Damage
intValue: 5
floatValue: 0
weaponId: ""
```

### Assets/_Project/ScriptableObjects/Upgrades/Upgrade_Defense.asset
- Type: UpgradeData
```text
upgradeName: DEFENSE UP
description: Damage Taken -1
icon: None
upgradeType: Defense
intValue: 1
floatValue: 0
weaponId: ""
```

### Assets/_Project/ScriptableObjects/Upgrades/Upgrade_MagicBolt.asset
- Type: UpgradeData
```text
upgradeName: MAGIC BOLT
description: Unlocks a magic bolt that automatically attacks nearby enemies.
icon: None
upgradeType: Weapon Unlock
intValue: 0
floatValue: 0
weaponId: magic_bolt
```

### Assets/_Project/ScriptableObjects/Upgrades/Upgrade_MaxHealth.asset
- Type: UpgradeData
```text
upgradeName: MAX HEALTH UP
description: Max Health +10
icon: None
upgradeType: Max Health
intValue: 10
floatValue: 0
weaponId: ""
```

### Assets/_Project/ScriptableObjects/Upgrades/Upgrade_MoveSpeed.asset
- Type: UpgradeData
```text
upgradeName: MOVE SPEED UP
description: Move Speed +5%
icon: None
upgradeType: Move Speed
intValue: 0
floatValue: 0.05
weaponId: ""
```

### Assets/_Project/ScriptableObjects/Upgrades/Upgrade_PickupRange.asset
- Type: UpgradeData
```text
upgradeName: PICKUP RANGE UP
description: Pickup Range +15%
icon: None
upgradeType: Pickup Range
intValue: 0
floatValue: 0.15
weaponId: ""
```

## Prefabs Snapshot

### Assets/_Project/Prefabs/Enemies/Enemy.prefab
```text
- Enemy [Transform, SpriteRenderer, Rigidbody2D, BoxCollider2D, EnemyMovement, EnemyHealth, EnemyContactDamage]
```

### Assets/_Project/Prefabs/Enemies/Enemy_Bat_01.prefab
```text
- Enemy_Bat_01 [Transform, SpriteRenderer, Rigidbody2D, BoxCollider2D, EnemyMovement, EnemyHealth, EnemyContactDamage]
```

### Assets/_Project/Prefabs/Enemies/Enemy_Ghoul_01.prefab
```text
- Enemy_Ghoul_01 [Transform, SpriteRenderer, Rigidbody2D, BoxCollider2D, EnemyMovement, EnemyHealth, EnemyContactDamage]
```

### Assets/_Project/Prefabs/Items/ExpGem.prefab
```text
- ExpGem [Transform, SpriteRenderer, BoxCollider2D, ExpGem]
```

### Assets/_Project/Prefabs/Items/SmallHeal.prefab
```text
- SmallHeal [Transform, SpriteRenderer, BoxCollider2D, SmallHealPickup]
```

### Assets/_Project/Prefabs/Projectiles/MagicBolt.prefab
```text
- MagicBolt [Transform, SpriteRenderer, CircleCollider2D, MagicBoltProjectile]
```

## File Change Summary

- Added: 0
- Modified: 0
- Deleted: 0
- Total tracked files: 66

- 변경된 파일 없음

## Code Change Preview

- 변경된 C# 코드 없음

## Scripts Tree

```text
- Assets/_Project/Scripts/Core/CameraFollow2D.cs
- Assets/_Project/Scripts/Core/GameFlowManager.cs
- Assets/_Project/Scripts/Core/GameTimer.cs
- Assets/_Project/Scripts/Core/PauseManager.cs
- Assets/_Project/Scripts/Core/StageTileGenerator.cs
- Assets/_Project/Scripts/Data/UpgradeData.cs
- Assets/_Project/Scripts/Debug/HUDDebugUI.cs
- Assets/_Project/Scripts/Editor/DevSnapshotTool.cs
- Assets/_Project/Scripts/Editor/EnemySpawnerEditor.cs
- Assets/_Project/Scripts/Enemy/EnemyContactDamage.cs
- Assets/_Project/Scripts/Enemy/EnemyHealth.cs
- Assets/_Project/Scripts/Enemy/EnemyMovement.cs
- Assets/_Project/Scripts/Items/ExpGem.cs
- Assets/_Project/Scripts/Items/SmallHealPickup.cs
- Assets/_Project/Scripts/Player/PlayerController.cs
- Assets/_Project/Scripts/Player/PlayerExp.cs
- Assets/_Project/Scripts/Player/PlayerHealth.cs
- Assets/_Project/Scripts/Player/PlayerPickupRange.cs
- Assets/_Project/Scripts/Projectiles/MagicBoltProjectile.cs
- Assets/_Project/Scripts/Relics/PlayerRelicEffects.cs
- Assets/_Project/Scripts/Relics/RelicData.cs
- Assets/_Project/Scripts/Spawning/EnemySpawner.cs
- Assets/_Project/Scripts/Spawning/WaveManager.cs
- Assets/_Project/Scripts/UI/HUDCanvasUI.cs
- Assets/_Project/Scripts/UI/LevelUpUI.cs
- Assets/_Project/Scripts/UI/RelicSelectUI.cs
- Assets/_Project/Scripts/Weapons/PlayerMagicBoltAutoAttack.cs
- Assets/_Project/Scripts/Weapons/PlayerMeleeAutoAttack.cs
```

## Current Script Structure

### Assets/_Project/Scripts/Core/CameraFollow2D.cs
```text
Classes:
- CameraFollow2D
Methods:
- LateUpdate()
```


### Assets/_Project/Scripts/Core/GameFlowManager.cs
```text
Classes:
- GameFlowManager
Methods:
- Start()
- Update()
- ShowStartMenu()
- StartGame()
- GameOver()
- RestartGame()
- QuitGame()
- ValidateRequiredReferences()
- RequestPause()
- ReleasePause()
```


### Assets/_Project/Scripts/Core/GameTimer.cs
```text
Classes:
- GameTimer
Methods:
- Update()
- StartTimer()
- StopTimer()
- ResetTimer()
- GetFormattedGameplayTime()
- UpdateTimerText()
- UpdateDebugLog()
```


### Assets/_Project/Scripts/Core/PauseManager.cs
```text
Classes:
- PauseManager
Methods:
- RequestPause()
- ReleasePause()
- ClearAllPauseRequests()
- OnDestroy()
- UpdateTimeScale()
```


### Assets/_Project/Scripts/Core/StageTileGenerator.cs
```text
Classes:
- StageTileGenerator
Methods:
- Start()
- Update()
- GetPlayerChunk()
- GenerateChunksAroundPlayer()
- GenerateChunk()
- PickGroundTile()
- GetStableRandom01()
- RemoveFarChunks()
- ClearChunk()
```


### Assets/_Project/Scripts/Data/UpgradeData.cs
```text
Enums:
- UpgradeType
Classes:
- UpgradeData
```


### Assets/_Project/Scripts/Debug/HUDDebugUI.cs
```text
Classes:
- HUDDebugUI
Methods:
- OnGUI()
- DrawHealth()
- DrawExp()
- DrawWave()
- DrawGameOver()
```


### Assets/_Project/Scripts/Editor/DevSnapshotTool.cs
```text
Classes:
- DevSnapshotTool
- SnapshotCache
- FileSnapshot
- DiagnosticIssue
- SceneObjectInfo
- SerializedFieldLine
Methods:
- GenerateDevSnapshot()
- ResetBaseline()
- BuildReport()
- AppendHeader()
- AppendUsageProtocol()
- AppendHardRules()
- AppendExecutiveSummary()
- AppendDetectedSystem()
- AppendDevState()
- AppendDiagnostics()
- AppendDiagnosticsBySeverity()
- AppendRecommendedNextSteps()
- BuildRecommendations()
- AppendArchitectureMap()
- AppendSceneHierarchy()
- AppendGameObjectTree()
- AppendImportantSceneObjects()
- AppendUiStructure()
- AppendScriptableObjects()
- AppendPrefabSummary()
- AppendPrefabTree()
- AppendFileChangeSummary()
- AppendCodeChangePreview()
- AppendScriptsTree()
- AppendCurrentScriptStructure()
- AppendFullSourceCode()
- BuildDiagnostics()
- DetectMissingScripts()
- DetectDuplicateImportantObjects()
- DetectEmptyImportantObjects()
- DetectDebugAndOfficialCoexistence()
- DetectSerializedReferenceProblems()
- DetectScriptableObjectProblems()
- DetectPrefabProblems()
- DetectCodeSmells()
- DetectKnownProjectRisks()
- CollectSceneObjectInfos()
- CollectSceneObjectInfoRecursive()
- AppendTransformInfo()
- AppendSerializedObjectFields()
- GetSerializedFieldLines()
- GetSerializedPropertyValue()
- ShouldSkipVerboseBuiltInComponent()
- IsCriticalComponent()
- IsOptionalReferenceField()
- GetGameObjectPath()
- GetRelativePrefabPath()
- CollectGameObjects()
- AppendFileList()
- AppendCodeStructure()
- AppendSimpleDiff()
- CountOccurrences()
- ScanProjectFiles()
- ShouldStoreText()
- SafeReadText()
- LoadCache()
- SaveCache()
- EnsureDocsFolder()
- SplitLines()
- GetSha256()
- NormalizePath()
- NormalizePathStatic()
```


### Assets/_Project/Scripts/Editor/EnemySpawnerEditor.cs
```text
Classes:
- EnemySpawnerEditor
Methods:
- OnEnable()
- OnInspectorGUI()
- DrawReferences()
- DrawEnemySpawnTable()
- DrawSpawnSettings()
- DrawHeader()
- DrawElement()
- AddEnemyEntry()
- GetTotalWeight()
```


### Assets/_Project/Scripts/Enemy/EnemyContactDamage.cs
```text
Classes:
- EnemyContactDamage
Methods:
- Initialize()
- Update()
- OnTriggerStay2D()
```


### Assets/_Project/Scripts/Enemy/EnemyHealth.cs
```text
Classes:
- EnemyHealth
Methods:
- Awake()
- Initialize()
- ApplyVisual()
- TakeDamage()
- PlayHitFlash()
- HitFlashRoutine()
- Die()
- NotifyEnemyKilled()
- DropExpGem()
- DropSmallHeal()
- GetExpGemDropPosition()
- GetSmallHealDropPosition()
- OpenRelicSelectUI()
```


### Assets/_Project/Scripts/Enemy/EnemyMovement.cs
```text
Classes:
- EnemyMovement
Methods:
- OnEnable()
- OnDisable()
- Awake()
- FixedUpdate()
- SetTarget()
- ApplyKnockback()
- UpdateTargetOffsetTimer()
- PickNewTargetOffset()
- MoveToTargetArea()
- GetSeparationDirection()
- OnDrawGizmosSelected()
```


### Assets/_Project/Scripts/Items/ExpGem.cs
```text
Classes:
- ExpGem
Methods:
- SetExpAmount()
- OnTriggerEnter2D()
- TryCollect()
```


### Assets/_Project/Scripts/Items/SmallHealPickup.cs
```text
Classes:
- SmallHealPickup
Methods:
- OnTriggerEnter2D()
- TryCollect()
```


### Assets/_Project/Scripts/Player/PlayerController.cs
```text
Classes:
- PlayerController
Methods:
- Awake()
- Update()
- FixedUpdate()
- ReadMoveInput()
- UpdateFacingDirection()
- Move()
- ImproveMoveSpeed()
- ApplyKnockback()
```


### Assets/_Project/Scripts/Player/PlayerExp.cs
```text
Classes:
- PlayerExp
Methods:
- AddExp()
- LevelUp()
- NotifyExpChanged()
```


### Assets/_Project/Scripts/Player/PlayerHealth.cs
```text
Classes:
- PlayerHealth
Methods:
- Awake()
- TakeDamage()
- AddMaxHealth()
- Heal()
- AddDefense()
- InvincibleRoutine()
- Die()
- NotifyHealthChanged()
```


### Assets/_Project/Scripts/Player/PlayerPickupRange.cs
```text
Classes:
- PlayerPickupRange
Methods:
- Awake()
- Update()
- ScanForExpGems()
- ImprovePickupRange()
- OnDrawGizmosSelected()
```


### Assets/_Project/Scripts/Projectiles/MagicBoltProjectile.cs
```text
Classes:
- MagicBoltProjectile
Methods:
- SetOwner()
- Initialize()
- Update()
- OnTriggerEnter2D()
- OnDisable()
- Release()
```


### Assets/_Project/Scripts/Relics/PlayerRelicEffects.cs
```text
Classes:
- PlayerRelicEffects
Methods:
- Awake()
- Update()
- ActivateBerserkerFang()
- ActivateBloodSigil()
- OnEnemyKilled()
- UpdateBerserkerFang()
```


### Assets/_Project/Scripts/Relics/RelicData.cs
```text
Enums:
- RelicType
Classes:
- RelicData
```


### Assets/_Project/Scripts/Spawning/EnemySpawner.cs
```text
Classes:
- EnemySpawner
- EnemySpawnEntry
Methods:
- Awake()
- Update()
- HasValidEnemyPrefab()
- TrySpawnEnemy()
- SpawnEnemy()
- DespawnEnemy()
- GetRandomEnemyPrefab()
- GetTotalSpawnWeight()
- IsValidEntry()
- GetSpawnPositionAroundPlayer()
- RemoveDeadEnemies()
- BuildEnemyPool()
- GetOrCreateEnemy()
```


### Assets/_Project/Scripts/Spawning/WaveManager.cs
```text
Classes:
- WaveManager
Methods:
- Awake()
- Update()
- AdvanceWave()
- ShouldSpawnFirstMidBoss()
- MarkFirstMidBossSpawned()
- IsMidBossWave()
- GetEnemyHealth()
- GetEnemyDamage()
- GetExpReward()
```


### Assets/_Project/Scripts/UI/HUDCanvasUI.cs
```text
Classes:
- HUDCanvasUI
Methods:
- Awake()
- OnEnable()
- OnDisable()
- Update()
- BindEvents()
- UnbindEvents()
- RefreshAllImmediate()
- UpdateHpUI()
- UpdateExpUI()
- UpdateWaveUI()
- UpdateRelicUI()
```


### Assets/_Project/Scripts/UI/LevelUpUI.cs
```text
Classes:
- LevelUpUI
Methods:
- Awake()
- ValidateRequiredReferences()
- AutoBindIfNeeded()
- FindTMP()
- FindButton()
- RegisterButtonEvents()
- Open()
- PickChoices()
- CanAppearInChoices()
- ChooseUpgrade()
- CanChoose()
- ApplyUpgrade()
- UnlockWeapon()
- CompleteOneChoice()
- OnDisable()
- RequestPause()
- ReleasePause()
- ClosePanelOnly()
- UpdateTexts()
- SetButtonText()
```


### Assets/_Project/Scripts/UI/RelicSelectUI.cs
```text
Classes:
- RelicSelectUI
Methods:
- Awake()
- ValidateRequiredReferences()
- AutoBindIfNeeded()
- FindTMP()
- FindButton()
- RegisterButtonEvents()
- Open()
- PickChoices()
- SelectRelic()
- OnDisable()
- RequestPause()
- ReleasePause()
- ApplyRelicEffect()
- UpdateTexts()
- SetButtonText()
- ClosePanelOnly()
```


### Assets/_Project/Scripts/Weapons/PlayerMagicBoltAutoAttack.cs
```text
Classes:
- PlayerMagicBoltAutoAttack
Methods:
- Awake()
- ValidateRequiredReferences()
- Update()
- Unlock()
- AddDamage()
- ImproveAttackSpeed()
- ImproveAttackRange()
- FindNearestEnemy()
- Fire()
- ReturnProjectileToPool()
- PrewarmProjectilePool()
- GetOrCreateProjectile()
- CreateProjectileInstance()
- OnDrawGizmosSelected()
```


### Assets/_Project/Scripts/Weapons/PlayerMeleeAutoAttack.cs
```text
Classes:
- PlayerMeleeAutoAttack
Methods:
- Awake()
- Update()
- AddDamage()
- ImproveAttackSpeed()
- ImproveAttackRange()
- AddCriticalChance()
- SetRelicAttackSpeedBonus()
- GetCurrentAttackInterval()
- Attack()
- CalculateDamage()
- ShowAttackVisual()
- AttackVisualRoutine()
- OnDrawGizmosSelected()
```


## Full Source Code

### FILE: Assets/_Project/Scripts/Core/CameraFollow2D.cs

```csharp
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
```

### FILE: Assets/_Project/Scripts/Core/GameFlowManager.cs

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private PauseManager pauseManager;

    [Header("UI")]
    [SerializeField] private GameObject startMenuCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject hudCanvas;

    [Header("Settings")]
    [SerializeField] private bool showStartMenuOnLoad = true;

    private bool isGameStarted;
    private bool isGameOver;

    private void Start()
    {
        ValidateRequiredReferences();

        if (gameTimer != null)
        {
            gameTimer.ResetTimer();
            gameTimer.StopTimer();
        }

        if (showStartMenuOnLoad)
        {
            ShowStartMenu();
        }
        else
        {
            StartGame();
        }
    }

    private void Update()
    {
        if (!isGameStarted)
            return;

        if (isGameOver)
            return;

        if (playerHealth != null && playerHealth.IsDead)
        {
            GameOver();
        }
    }

    private void ShowStartMenu()
    {
        isGameStarted = false;
        isGameOver = false;

        RequestPause();

        if (gameTimer != null)
        {
            gameTimer.ResetTimer();
            gameTimer.StopTimer();
        }

        if (startMenuCanvas != null)
            startMenuCanvas.SetActive(true);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        if (hudCanvas != null)
            hudCanvas.SetActive(false);
    }

    public void StartGame()
    {
        isGameStarted = true;
        isGameOver = false;

        ReleasePause();

        if (gameTimer != null)
        {
            gameTimer.ResetTimer();
            gameTimer.StartTimer();
        }

        if (startMenuCanvas != null)
            startMenuCanvas.SetActive(false);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        if (hudCanvas != null)
            hudCanvas.SetActive(true);
    }

    public void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        if (gameTimer != null)
        {
            gameTimer.StopTimer();
        }

        RequestPause();

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);
    }

    public void RestartGame()
    {
        if (pauseManager != null)
        {
            pauseManager.ClearAllPauseRequests();
        }
        else
        {
            Time.timeScale = 1f;
        }

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ValidateRequiredReferences()
    {
        if (pauseManager == null)
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        if (playerHealth == null)
        {
            Debug.LogWarning("[GameFlowManager] playerHealth가 비어 있습니다. PlayerHealth 연결이 없으면 게임오버 감지가 동작하지 않을 수 있습니다.", this);
        }

        if (gameTimer == null)
        {
            Debug.LogWarning("[GameFlowManager] gameTimer가 비어 있습니다. 타이머 UI와 시간 측정이 동작하지 않을 수 있습니다.", this);
        }

        if (startMenuCanvas == null || gameOverCanvas == null || hudCanvas == null)
        {
            Debug.LogWarning("[GameFlowManager] UI Canvas 참조가 일부 비어 있습니다. startMenuCanvas / gameOverCanvas / hudCanvas 연결을 확인하세요.", this);
        }

        if (pauseManager == null)
        {
            Debug.LogWarning("[GameFlowManager] pauseManager가 비어 있습니다. PauseManager 연결을 권장합니다. (없으면 Time.timeScale 폴백 사용)", this);
        }
    }

    private void RequestPause()
    {
        if (pauseManager != null)
        {
            pauseManager.RequestPause(this);
            return;
        }

        Time.timeScale = 0f;
    }

    private void ReleasePause()
    {
        if (pauseManager != null)
        {
            pauseManager.ReleasePause(this);
            return;
        }

        Time.timeScale = 1f;
    }
}
```

### FILE: Assets/_Project/Scripts/Core/GameTimer.cs

```csharp
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private bool isRunning;

    [Header("Time")]
    [SerializeField] private float gameplayTime;
    [SerializeField] private float realTime;
    [SerializeField] private float logTimer;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;

    [Header("Debug")]
    [SerializeField] private bool showDebugLog;
    [SerializeField] private float logInterval = 10f;

    public float GameplayTime => gameplayTime;
    public float RealTime => realTime;
    public int GameplayMinutes => Mathf.FloorToInt(gameplayTime / 60f);
    public int GameplaySeconds => Mathf.FloorToInt(gameplayTime % 60f);

    private void Update()
    {
        realTime += Time.unscaledDeltaTime;

        if (!isRunning)
        {
            UpdateTimerText();
            return;
        }

        gameplayTime += Time.deltaTime;
        logTimer += Time.unscaledDeltaTime;

        UpdateTimerText();
        UpdateDebugLog();
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        gameplayTime = 0f;
        realTime = 0f;
        logTimer = 0f;
        UpdateTimerText();
    }

    public string GetFormattedGameplayTime()
    {
        int minutes = Mathf.FloorToInt(gameplayTime / 60f);
        int seconds = Mathf.FloorToInt(gameplayTime % 60f);

        return $"{minutes:00}:{seconds:00}";
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        timerText.text = GetFormattedGameplayTime();
    }

    private void UpdateDebugLog()
    {
        if (!showDebugLog)
            return;

        if (logTimer < logInterval)
            return;

        logTimer = 0f;

        Debug.Log($"[GameTimer] Gameplay: {GetFormattedGameplayTime()} / Real: {realTime:F1}s");
    }
}
```

### FILE: Assets/_Project/Scripts/Core/PauseManager.cs

```csharp
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private readonly HashSet<Object> pauseRequesters = new HashSet<Object>();

    public bool IsPaused => pauseRequesters.Count > 0;
    public int PauseRequestCount => pauseRequesters.Count;

    public void RequestPause(Object requester)
    {
        if (requester == null)
        {
            Debug.LogWarning("[PauseManager] requester가 null이라 일시정지 요청이 무시되었습니다.", this);
            return;
        }

        pauseRequesters.Add(requester);
        UpdateTimeScale();
    }

    public void ReleasePause(Object requester)
    {
        if (requester == null)
        {
            return;
        }

        pauseRequesters.Remove(requester);
        UpdateTimeScale();
    }

    public void ClearAllPauseRequests()
    {
        pauseRequesters.Clear();
        UpdateTimeScale();
    }

    private void OnDestroy()
    {
        // 씬 전환/종료 시 게임이 멈춘 상태로 남지 않도록 안전하게 복구합니다.
        Time.timeScale = 1f;
    }

    private void UpdateTimeScale()
    {
        Time.timeScale = IsPaused ? 0f : 1f;
    }
}

```

### FILE: Assets/_Project/Scripts/Core/StageTileGenerator.cs

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageTileGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Tilemap groundTilemap;

    [Header("Ground Tiles")]
    [SerializeField] private TileBase groundDirt01;
    [SerializeField] private TileBase groundDirt02;
    [SerializeField] private TileBase groundGrass01;
    [SerializeField] private TileBase groundGrass02;
    [SerializeField] private TileBase groundStone01;
    [SerializeField] private TileBase groundCrack01;

    [Header("Generation Settings")]
    [SerializeField] private int chunkSize = 16;
    [SerializeField] private int activeChunkRadius = 2;
    [SerializeField] private int seed = 12345;
    [SerializeField] private bool clearExistingTilesOnStart = true;

    private readonly HashSet<Vector2Int> generatedChunks = new HashSet<Vector2Int>();
    private Vector2Int currentPlayerChunk;

    private void Start()
    {
        if (player == null || groundTilemap == null)
        {
            Debug.LogError("StageTileGenerator: Player 또는 Ground Tilemap이 연결되지 않았습니다.");
            enabled = false;
            return;
        }

        if (clearExistingTilesOnStart)
        {
            groundTilemap.ClearAllTiles();
        }

        currentPlayerChunk = GetPlayerChunk();
        GenerateChunksAroundPlayer();
    }

    private void Update()
    {
        Vector2Int newPlayerChunk = GetPlayerChunk();

        if (newPlayerChunk == currentPlayerChunk)
            return;

        currentPlayerChunk = newPlayerChunk;

        GenerateChunksAroundPlayer();
        RemoveFarChunks();
    }

    private Vector2Int GetPlayerChunk()
    {
        Vector3Int playerCell = groundTilemap.WorldToCell(player.position);

        int chunkX = Mathf.FloorToInt((float)playerCell.x / chunkSize);
        int chunkY = Mathf.FloorToInt((float)playerCell.y / chunkSize);

        return new Vector2Int(chunkX, chunkY);
    }

    private void GenerateChunksAroundPlayer()
    {
        for (int y = -activeChunkRadius; y <= activeChunkRadius; y++)
        {
            for (int x = -activeChunkRadius; x <= activeChunkRadius; x++)
            {
                Vector2Int chunkPosition = currentPlayerChunk + new Vector2Int(x, y);

                if (generatedChunks.Contains(chunkPosition))
                    continue;

                GenerateChunk(chunkPosition);
                generatedChunks.Add(chunkPosition);
            }
        }
    }

    private void GenerateChunk(Vector2Int chunkPosition)
    {
        int startX = chunkPosition.x * chunkSize;
        int startY = chunkPosition.y * chunkSize;

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                int tileX = startX + x;
                int tileY = startY + y;

                TileBase selectedTile = PickGroundTile(tileX, tileY);

                Vector3Int cellPosition = new Vector3Int(tileX, tileY, 0);
                groundTilemap.SetTile(cellPosition, selectedTile);
            }
        }
    }

    private TileBase PickGroundTile(int x, int y)
    {
        float value = GetStableRandom01(x, y);

        if (value < 0.55f)
            return groundDirt01;

        if (value < 0.75f)
            return groundDirt02;

        if (value < 0.83f)
            return groundGrass01;

        if (value < 0.88f)
            return groundGrass02;

        if (value < 0.96f)
            return groundStone01;

        return groundCrack01;
    }

    private float GetStableRandom01(int x, int y)
    {
        int hash = seed;
        hash ^= x * 73856093;
        hash ^= y * 19349663;
        hash = (hash << 13) ^ hash;

        int result = (hash * (hash * hash * 15731 + 789221) + 1376312589);
        result &= 0x7fffffff;

        return result / 2147483647f;
    }

    private void RemoveFarChunks()
    {
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();

        foreach (Vector2Int chunk in generatedChunks)
        {
            int distanceX = Mathf.Abs(chunk.x - currentPlayerChunk.x);
            int distanceY = Mathf.Abs(chunk.y - currentPlayerChunk.y);

            if (distanceX > activeChunkRadius || distanceY > activeChunkRadius)
            {
                chunksToRemove.Add(chunk);
            }
        }

        foreach (Vector2Int chunk in chunksToRemove)
        {
            ClearChunk(chunk);
            generatedChunks.Remove(chunk);
        }
    }

    private void ClearChunk(Vector2Int chunkPosition)
    {
        int startX = chunkPosition.x * chunkSize;
        int startY = chunkPosition.y * chunkSize;

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                Vector3Int cellPosition = new Vector3Int(startX + x, startY + y, 0);
                groundTilemap.SetTile(cellPosition, null);
            }
        }
    }
}
```

### FILE: Assets/_Project/Scripts/Data/UpgradeData.cs

```csharp
using UnityEngine;

public enum UpgradeType
{
    Damage,
    AttackSpeed,
    AttackRange,
    MaxHealth,
    MoveSpeed,
    PickupRange,
    Defense,
    CriticalChance,
    WeaponUnlock
}

[CreateAssetMenu(fileName = "Upgrade_", menuName = "_Project/Upgrades/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string upgradeName;
    [SerializeField, TextArea(2, 4)] private string description;
    [SerializeField] private Sprite icon;

    [Header("Effect")]
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private int intValue;
    [SerializeField] private float floatValue;

    [Header("Weapon Unlock")]
    [SerializeField] private string weaponId;

    public string UpgradeName => upgradeName;
    public string Description => description;
    public Sprite Icon => icon;

    public UpgradeType UpgradeType => upgradeType;
    public int IntValue => intValue;
    public float FloatValue => floatValue;

    public string WeaponId => weaponId;
}
```

### FILE: Assets/_Project/Scripts/Debug/HUDDebugUI.cs

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDDebugUI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerExp playerExp;
    [SerializeField] private WaveManager waveManager;

    [Header("Layout")]
    [SerializeField] private int x = 20;
    [SerializeField] private int y = 20;
    [SerializeField] private int width = 260;
    [SerializeField] private int height = 24;

    private void OnGUI()
    {
        DrawHealth();
        DrawExp();
        DrawWave();
        DrawGameOver();
    }

    private void DrawHealth()
    {
        if (playerHealth == null)
            return;

        float hpRatio = 0f;

        if (playerHealth.MaxHealth > 0)
        {
            hpRatio = (float)playerHealth.CurrentHealth / playerHealth.MaxHealth;
        }

        GUI.Box(new Rect(x, y, width, height), "");
        GUI.Box(new Rect(x, y, width * hpRatio, height), "");

        GUI.Label(
            new Rect(x + 8, y + 3, width, height),
            $"HP {playerHealth.CurrentHealth} / {playerHealth.MaxHealth}"
        );
    }

    private void DrawExp()
    {
        if (playerExp == null)
            return;

        int expY = y + height + 8;

        float expRatio = 0f;

        if (playerExp.ExpToNextLevel > 0)
        {
            expRatio = (float)playerExp.CurrentExp / playerExp.ExpToNextLevel;
        }

        GUI.Box(new Rect(x, expY, width, height), "");
        GUI.Box(new Rect(x, expY, width * expRatio, height), "");

        GUI.Label(
            new Rect(x + 8, expY + 3, width, height),
            $"LV {playerExp.Level}  EXP {playerExp.CurrentExp} / {playerExp.ExpToNextLevel}"
        );
    }

    private void DrawWave()
    {
        if (waveManager == null)
            return;

        int waveY = y + (height + 8) * 2;

        string label = waveManager.IsMidBossWave()
            ? $"WAVE {waveManager.CurrentWave}  MID BOSS"
            : $"WAVE {waveManager.CurrentWave}  NEXT {Mathf.CeilToInt(waveManager.WaveTimer)}s";

        GUI.Box(new Rect(x, waveY, width, height), label);
    }

    private void DrawGameOver()
    {
        if (playerHealth == null || !playerHealth.IsDead)
            return;

        int boxWidth = 320;
        int boxHeight = 160;
        int boxX = (Screen.width - boxWidth) / 2;
        int boxY = (Screen.height - boxHeight) / 2;

        GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), "GAME OVER");

        GUI.Label(
            new Rect(boxX + 40, boxY + 45, boxWidth - 80, 30),
            "플레이어가 사망했습니다."
        );

        if (GUI.Button(new Rect(boxX + 60, boxY + 95, boxWidth - 120, 40), "다시 시작"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
```

### FILE: Assets/_Project/Scripts/Editor/DevSnapshotTool.cs

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DevSnapshotTool
{
    private const string ProjectRoot = "Assets/_Project";
    private const string DocsFolder = "Assets/_Project/Docs";
    private const string SnapshotPath = "Assets/_Project/Docs/DevSnapshot_Expert.md";
    private const string GptContextPath = "Assets/_Project/Docs/GPT_CONTEXT.md";
    private const string DevStatePath = "Assets/_Project/Docs/DevState.md";
    private const string CachePath = "Library/DevSnapshotCache.json";

    private const int MaxDiffLines = 100;
    private const int MaxSerializedPropertiesPerComponent = 140;
    private const int MaxIssueCountPerSection = 80;

    private static readonly string[] TargetExtensions =
    {
        ".cs",
        ".unity",
        ".prefab",
        ".asset",
        ".controller",
        ".anim",
        ".png",
        ".jpg",
        ".jpeg",
        ".webp"
    };

    private static readonly string[] GeneratedOutputPaths =
    {
        NormalizePathStatic(SnapshotPath),
        NormalizePathStatic(GptContextPath)
    };

    private static readonly string[] ImportantObjectNameKeywords =
    {
        "Player",
        "EnemySpawner",
        "WaveManager",
        "GameTimer",
        "GameFlowManager",
        "HUDCanvas",
        "HUDPanel",
        "LevelUpCanvas",
        "LevelUpUI",
        "RelicSelectUI",
        "RelicSelectPanel",
        "StartMenuCanvas",
        "GameOverCanvas",
        "StageTileGenerator",
        "GroundGrid",
        "GroundTilemap",
        "EventSystem"
    };

    private static readonly string[] DebugNameKeywords =
    {
        "Debug",
        "DebugUI",
        "Test",
        "Temp",
        "Sample"
    };

    private static readonly string[] OptionalReferenceKeywords =
    {
        "icon",
        "sprite",
        "sourceImage",
        "material",
        "font",
        "audio",
        "sfx",
        "bgm",
        "tint",
        "preview"
    };

    [Serializable]
    private class SnapshotCache
    {
        public List<FileSnapshot> files = new List<FileSnapshot>();
    }

    [Serializable]
    private class FileSnapshot
    {
        public string path;
        public string hash;
        public string text;
        public long size;
    }

    private class DiagnosticIssue
    {
        public string severity;
        public string title;
        public string detail;
        public string suggestion;

        public DiagnosticIssue(string severity, string title, string detail, string suggestion)
        {
            this.severity = severity;
            this.title = title;
            this.detail = detail;
            this.suggestion = suggestion;
        }
    }

    private class SceneObjectInfo
    {
        public GameObject gameObject;
        public string path;
        public string name;
        public bool activeSelf;
        public string tag;
        public int layer;
        public List<string> componentNames = new List<string>();
        public List<Component> components = new List<Component>();
    }

    private class SerializedFieldLine
    {
        public string path;
        public string value;
        public int depth;
        public SerializedPropertyType type;
        public bool isNullObjectReference;
        public bool isOptionalReference;
    }

    [MenuItem("_Project/Dev Tools/Generate Dev Snapshot")]
    public static void GenerateDevSnapshot()
    {
        EnsureDocsFolder();

        SnapshotCache previousCache = LoadCache();
        Dictionary<string, FileSnapshot> previousMap = previousCache.files.ToDictionary(f => f.path, f => f);

        List<FileSnapshot> currentFiles = ScanProjectFiles();
        Dictionary<string, FileSnapshot> currentMap = currentFiles.ToDictionary(f => f.path, f => f);

        List<FileSnapshot> addedFiles = new List<FileSnapshot>();
        List<FileSnapshot> modifiedFiles = new List<FileSnapshot>();
        List<FileSnapshot> deletedFiles = new List<FileSnapshot>();

        foreach (FileSnapshot current in currentFiles)
        {
            if (!previousMap.TryGetValue(current.path, out FileSnapshot previous))
            {
                addedFiles.Add(current);
                continue;
            }

            if (previous.hash != current.hash)
            {
                modifiedFiles.Add(current);
            }
        }

        foreach (FileSnapshot previous in previousCache.files)
        {
            if (!currentMap.ContainsKey(previous.path))
            {
                deletedFiles.Add(previous);
            }
        }

        string report = BuildReport(
            previousMap,
            currentFiles,
            addedFiles,
            modifiedFiles,
            deletedFiles
        );

        File.WriteAllText(SnapshotPath, report, Encoding.UTF8);
        File.WriteAllText(GptContextPath, report, Encoding.UTF8);

        SaveCache(currentFiles);

        EditorGUIUtility.systemCopyBuffer = report;
        AssetDatabase.Refresh();

        Debug.Log($"Expert Dev Snapshot 생성 완료: {SnapshotPath}");
        Debug.Log($"GPT Context 생성 완료: {GptContextPath}");
        Debug.Log("리포트 전체가 클립보드에 복사되었습니다.");
    }

    [MenuItem("_Project/Dev Tools/Reset Dev Snapshot Baseline")]
    public static void ResetBaseline()
    {
        List<FileSnapshot> currentFiles = ScanProjectFiles();
        SaveCache(currentFiles);

        Debug.Log("Dev Snapshot 기준점이 현재 상태로 초기화되었습니다. 다음 Generate부터 변경분 기준으로 표시됩니다.");
    }

    private static string BuildReport(
        Dictionary<string, FileSnapshot> previousMap,
        List<FileSnapshot> currentFiles,
        List<FileSnapshot> addedFiles,
        List<FileSnapshot> modifiedFiles,
        List<FileSnapshot> deletedFiles)
    {
        List<SceneObjectInfo> sceneObjects = CollectSceneObjectInfos();
        List<DiagnosticIssue> issues = BuildDiagnostics(currentFiles, sceneObjects);

        StringBuilder sb = new StringBuilder();

        AppendHeader(sb);
        AppendUsageProtocol(sb);
        AppendHardRules(sb);
        AppendExecutiveSummary(sb, currentFiles, addedFiles, modifiedFiles, deletedFiles, sceneObjects, issues);
        AppendDevState(sb);
        AppendDiagnostics(sb, issues);
        AppendRecommendedNextSteps(sb, currentFiles, sceneObjects, issues);
        AppendArchitectureMap(sb, currentFiles, sceneObjects);
        AppendSceneHierarchy(sb, sceneObjects);
        AppendImportantSceneObjects(sb, sceneObjects);
        AppendUiStructure(sb, sceneObjects);
        AppendScriptableObjects(sb);
        AppendPrefabSummary(sb);
        AppendFileChangeSummary(sb, currentFiles, addedFiles, modifiedFiles, deletedFiles);
        AppendCodeChangePreview(sb, previousMap, addedFiles, modifiedFiles);
        AppendScriptsTree(sb, currentFiles);
        AppendCurrentScriptStructure(sb, currentFiles);
        AppendFullSourceCode(sb, currentFiles);

        return sb.ToString();
    }

    private static void AppendHeader(StringBuilder sb)
    {
        Scene activeScene = SceneManager.GetActiveScene();

        sb.AppendLine("# EXPERT GPT / DEV SNAPSHOT");
        sb.AppendLine();
        sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine($"Unity Version: {Application.unityVersion}");
        sb.AppendLine($"Active Scene: {activeScene.name}");
        sb.AppendLine($"Scene Path: {activeScene.path}");
        sb.AppendLine($"Project Root: {ProjectRoot}");
        sb.AppendLine();
    }

    private static void AppendUsageProtocol(StringBuilder sb)
    {
        sb.AppendLine("## HOW GPT MUST USE THIS SNAPSHOT");
        sb.AppendLine();
        sb.AppendLine("1. 먼저 `Executive Summary`, `Diagnostics`, `Scene Hierarchy`, `UI Structure`, `Important Scene Objects`를 읽는다.");
        sb.AppendLine("2. 새 스크립트나 새 GameObject를 제안하기 전에 기존 구조로 해결 가능한지 판단한다.");
        sb.AppendLine("3. 코드 작성 전 반드시 수정 대상 파일과 이유를 먼저 말한다.");
        sb.AppendLine("4. 사용자가 현재 구조를 묻는 경우 이 Snapshot 내용을 기준으로 답한다.");
        sb.AppendLine("5. 이 Snapshot에 없는 오브젝트/스크립트/연결을 있다고 가정하지 않는다.");
        sb.AppendLine();
    }

    private static void AppendHardRules(StringBuilder sb)
    {
        sb.AppendLine("## HARD RULES");
        sb.AppendLine();
        sb.AppendLine("- 기존 HUD 구조 확인 전 새 HUD Canvas/Panel/Script 생성 금지.");
        sb.AppendLine("- DebugUI와 정식 UI 혼동 금지.");
        sb.AppendLine("- 새 기능 추가 전 기존 Manager/UI/Data 구조 재사용 우선.");
        sb.AppendLine("- Inspector 연결이 필요한 경우 연결 대상 필드명과 오브젝트명을 반드시 명시.");
        sb.AppendLine("- 전체 코드 제공 시 파일명/경로/전체 교체 여부를 반드시 명시.");
        sb.AppendLine("- 오류가 있으면 추측하지 말고 Console 로그/파일/Hierarchy 기준으로 판단.");
        sb.AppendLine("- 한 번에 여러 시스템을 동시에 수정하지 말 것.");
        sb.AppendLine("- 새 파일 추가는 반드시 `왜 기존 파일 수정으로 안 되는지` 설명한 뒤 제안.");
        sb.AppendLine();
    }

    private static void AppendExecutiveSummary(
        StringBuilder sb,
        List<FileSnapshot> currentFiles,
        List<FileSnapshot> addedFiles,
        List<FileSnapshot> modifiedFiles,
        List<FileSnapshot> deletedFiles,
        List<SceneObjectInfo> sceneObjects,
        List<DiagnosticIssue> issues)
    {
        int scriptsCount = currentFiles.Count(f => f.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase));
        int prefabsCount = currentFiles.Count(f => f.path.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase));
        int assetsCount = currentFiles.Count(f => f.path.EndsWith(".asset", StringComparison.OrdinalIgnoreCase));

        int highCount = issues.Count(i => i.severity == "HIGH");
        int mediumCount = issues.Count(i => i.severity == "MEDIUM");
        int lowCount = issues.Count(i => i.severity == "LOW");

        sb.AppendLine("## Executive Summary");
        sb.AppendLine();
        sb.AppendLine($"- Scene Objects: {sceneObjects.Count}");
        sb.AppendLine($"- C# Scripts: {scriptsCount}");
        sb.AppendLine($"- Prefabs: {prefabsCount}");
        sb.AppendLine($"- Asset Files: {assetsCount}");
        sb.AppendLine($"- Changed Files: Added {addedFiles.Count}, Modified {modifiedFiles.Count}, Deleted {deletedFiles.Count}");
        sb.AppendLine($"- Diagnostics: HIGH {highCount}, MEDIUM {mediumCount}, LOW {lowCount}");
        sb.AppendLine();

        sb.AppendLine("### Key Existing Systems Detected");
        sb.AppendLine();

        AppendDetectedSystem(sb, currentFiles, sceneObjects, "HUD", "HUDCanvasUI.cs", "HUDCanvas");
        AppendDetectedSystem(sb, currentFiles, sceneObjects, "Level Up", "LevelUpUI.cs", "LevelUpCanvas");
        AppendDetectedSystem(sb, currentFiles, sceneObjects, "Relic Select", "RelicSelectUI.cs", "RelicSelectUI");
        AppendDetectedSystem(sb, currentFiles, sceneObjects, "Relic Effects", "PlayerRelicEffects.cs", "Player");
        AppendDetectedSystem(sb, currentFiles, sceneObjects, "Spawner", "EnemySpawner.cs", "EnemySpawner");
        AppendDetectedSystem(sb, currentFiles, sceneObjects, "Wave", "WaveManager.cs", "WaveManager");
        AppendDetectedSystem(sb, currentFiles, sceneObjects, "Timer", "GameTimer.cs", "GameTimer");

        sb.AppendLine();
    }

    private static void AppendDetectedSystem(
        StringBuilder sb,
        List<FileSnapshot> currentFiles,
        List<SceneObjectInfo> sceneObjects,
        string label,
        string scriptFileName,
        string sceneObjectName)
    {
        bool hasScript = currentFiles.Any(f => Path.GetFileName(f.path).Equals(scriptFileName, StringComparison.OrdinalIgnoreCase));
        bool hasObject = sceneObjects.Any(o => o.name.Equals(sceneObjectName, StringComparison.OrdinalIgnoreCase));

        string status = hasScript && hasObject ? "OK" : hasScript ? "Script Only" : hasObject ? "Object Only" : "Missing";

        sb.AppendLine($"- {label}: {status} (Script: {scriptFileName}={hasScript}, Object: {sceneObjectName}={hasObject})");
    }

    private static void AppendDevState(StringBuilder sb)
    {
        sb.AppendLine("## DevState.md");
        sb.AppendLine();

        if (!File.Exists(DevStatePath))
        {
            sb.AppendLine("- DevState.md 없음");
            sb.AppendLine();
            return;
        }

        string text = File.ReadAllText(DevStatePath, Encoding.UTF8);

        if (string.IsNullOrWhiteSpace(text))
        {
            sb.AppendLine("- DevState.md 비어 있음");
            sb.AppendLine();
            return;
        }

        sb.AppendLine("```markdown");
        sb.AppendLine(text.Trim());
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendDiagnostics(StringBuilder sb, List<DiagnosticIssue> issues)
    {
        sb.AppendLine("## Diagnostics");
        sb.AppendLine();

        if (issues.Count == 0)
        {
            sb.AppendLine("- 감지된 주요 위험 없음");
            sb.AppendLine();
            return;
        }

        AppendDiagnosticsBySeverity(sb, issues, "HIGH");
        AppendDiagnosticsBySeverity(sb, issues, "MEDIUM");
        AppendDiagnosticsBySeverity(sb, issues, "LOW");
    }

    private static void AppendDiagnosticsBySeverity(StringBuilder sb, List<DiagnosticIssue> issues, string severity)
    {
        List<DiagnosticIssue> filtered = issues.Where(i => i.severity == severity).Take(MaxIssueCountPerSection).ToList();

        if (filtered.Count == 0)
            return;

        sb.AppendLine($"### {severity}");
        sb.AppendLine();

        for (int i = 0; i < filtered.Count; i++)
        {
            DiagnosticIssue issue = filtered[i];

            sb.AppendLine($"{i + 1}. **{issue.title}**");
            sb.AppendLine($"   - Detail: {issue.detail}");
            sb.AppendLine($"   - Suggestion: {issue.suggestion}");
        }

        sb.AppendLine();

        int total = issues.Count(i => i.severity == severity);

        if (total > filtered.Count)
        {
            sb.AppendLine($"- ... {severity} issue {total - filtered.Count}개 추가 생략");
            sb.AppendLine();
        }
    }

    private static void AppendRecommendedNextSteps(
        StringBuilder sb,
        List<FileSnapshot> currentFiles,
        List<SceneObjectInfo> sceneObjects,
        List<DiagnosticIssue> issues)
    {
        sb.AppendLine("## Recommended Next Steps");
        sb.AppendLine();

        List<string> recommendations = BuildRecommendations(currentFiles, sceneObjects, issues);

        if (recommendations.Count == 0)
        {
            sb.AppendLine("1. 현재 구조 기준으로 명확한 우선 위험 없음. 다음 기능은 DevState.md 기준으로 진행.");
            sb.AppendLine();
            return;
        }

        for (int i = 0; i < recommendations.Count; i++)
        {
            sb.AppendLine($"{i + 1}. {recommendations[i]}");
        }

        sb.AppendLine();
    }

    private static List<string> BuildRecommendations(
        List<FileSnapshot> currentFiles,
        List<SceneObjectInfo> sceneObjects,
        List<DiagnosticIssue> issues)
    {
        List<string> result = new List<string>();

        bool hasHudCanvas = sceneObjects.Any(o => o.name == "HUDCanvas");
        bool hasHudPanel = sceneObjects.Any(o => o.name == "HUDPanel");
        bool hasHudCanvasUi = currentFiles.Any(f => f.path.EndsWith("HUDCanvasUI.cs", StringComparison.OrdinalIgnoreCase));
        bool hasRelicSelectUi = currentFiles.Any(f => f.path.EndsWith("RelicSelectUI.cs", StringComparison.OrdinalIgnoreCase));
        bool hasRelicDebugUi = currentFiles.Any(f => f.path.EndsWith("RelicSelectDebugUI.cs", StringComparison.OrdinalIgnoreCase));
        bool hasSmallHeal = currentFiles.Any(f => f.path.EndsWith("SmallHealPickup.cs", StringComparison.OrdinalIgnoreCase));
        bool hasEnemyHealth = currentFiles.Any(f => f.path.EndsWith("EnemyHealth.cs", StringComparison.OrdinalIgnoreCase));
        bool hasMeleeWeapon = currentFiles.Any(f => f.path.EndsWith("PlayerMeleeAutoAttack.cs", StringComparison.OrdinalIgnoreCase));

        if (hasHudCanvas && hasHudPanel && hasHudCanvasUi && hasRelicSelectUi)
        {
            result.Add("유물 보유 표시를 추가할 때는 새 HUD Canvas부터 만들지 말고, 기존 HUDCanvas/HUDPanel/HUDCanvasUI 구조를 먼저 검토한 뒤 통합 여부를 결정한다.");
        }

        if (hasSmallHeal && hasEnemyHealth)
        {
            FileSnapshot enemyHealth = currentFiles.FirstOrDefault(f => f.path.EndsWith("EnemyHealth.cs", StringComparison.OrdinalIgnoreCase));

            if (enemyHealth != null &&
                enemyHealth.text.Contains("DropExpGem") &&
                enemyHealth.text.Contains("DropSmallHeal") &&
                CountOccurrences(enemyHealth.text, "transform.position") >= 2)
            {
                result.Add("SmallHeal과 ExpGem이 같은 위치에 드랍될 가능성이 높다. 다음 작업은 EnemyHealth 드랍 위치 오프셋 분리로 잡는 것이 안전하다.");
            }
        }

        if (hasRelicDebugUi && hasRelicSelectUi)
        {
            result.Add("RelicSelectDebugUI와 RelicSelectUI가 공존한다. 신규 유물 UI 작업은 RelicSelectUI만 사용하고, DebugUI는 보류/삭제 후보로 분리한다.");
        }

        if (hasMeleeWeapon)
        {
            int weaponScriptCount = currentFiles.Count(f =>
                f.path.Contains("/Weapons/", StringComparison.OrdinalIgnoreCase) &&
                f.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase));

            if (weaponScriptCount <= 1)
            {
                result.Add("무기 스크립트가 근접 자동공격 1개 중심이다. 전투 단조로움 해결은 Brute 추가보다 MagicBolt 같은 두 번째 무기 구조를 곧 검토해야 한다.");
            }
        }

        if (issues.Any(i => i.severity == "HIGH"))
        {
            result.Insert(0, "HIGH 진단 항목이 있으므로 새 기능 추가 전에 연결 누락/중복/누락 스크립트부터 정리한다.");
        }

        if (result.Count == 0)
        {
            result.Add("다음 작업은 DevState.md의 우선순위를 따른다. 단, 새 구조 추가 전 기존 시스템 재사용 가능성을 먼저 판단한다.");
        }

        return result.Distinct().Take(10).ToList();
    }

    private static void AppendArchitectureMap(StringBuilder sb, List<FileSnapshot> currentFiles, List<SceneObjectInfo> sceneObjects)
    {
        sb.AppendLine("## Existing Systems To Reuse");
        sb.AppendLine();

        sb.AppendLine("### UI");
        sb.AppendLine("- HUD 작업: HUDCanvas, HUDPanel, HUDCanvasUI 우선 확인.");
        sb.AppendLine("- 레벨업 작업: LevelUpCanvas, LevelUpUI, UpgradeData 우선 활용.");
        sb.AppendLine("- 유물 선택 작업: RelicSelectUI, RelicData, PlayerRelicEffects 우선 활용.");
        sb.AppendLine("- DebugUI 파일은 신규 기능의 기준으로 삼지 말 것.");
        sb.AppendLine();

        sb.AppendLine("### Gameplay");
        sb.AppendLine("- 플레이어 체력/방어/회복: PlayerHealth.");
        sb.AppendLine("- 플레이어 이동/넉백: PlayerController.");
        sb.AppendLine("- 근접 자동공격/치명타: PlayerMeleeAutoAttack.");
        sb.AppendLine("- 경험치/레벨업: PlayerExp + LevelUpUI.");
        sb.AppendLine("- 적 체력/드랍/중간보스 사망 처리: EnemyHealth.");
        sb.AppendLine("- 적 스폰/중간보스 스폰: EnemySpawner + WaveManager.");
        sb.AppendLine();

        sb.AppendLine("### Data");
        sb.AppendLine("- 강화 데이터: UpgradeData ScriptableObject.");
        sb.AppendLine("- 유물 데이터: RelicData ScriptableObject.");
        sb.AppendLine();
    }

    private static void AppendSceneHierarchy(StringBuilder sb, List<SceneObjectInfo> sceneObjects)
    {
        sb.AppendLine("## Current Scene Hierarchy");
        sb.AppendLine();

        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] roots = activeScene.GetRootGameObjects();

        sb.AppendLine("```text");

        foreach (GameObject root in roots)
        {
            AppendGameObjectTree(sb, root.transform, 0);
        }

        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendGameObjectTree(StringBuilder sb, Transform transform, int depth)
    {
        if (transform == null)
            return;

        string indent = new string(' ', depth * 2);
        GameObject go = transform.gameObject;
        string activeText = go.activeSelf ? "" : " (inactive)";
        string components = string.Join(", ", go.GetComponents<Component>()
            .Where(c => c != null)
            .Select(c => c.GetType().Name));

        sb.AppendLine($"{indent}- {go.name}{activeText} [{components}]");

        Component[] comps = go.GetComponents<Component>();

        foreach (Component component in comps)
        {
            if (component == null)
            {
                sb.AppendLine($"{indent}  ! Missing Script");
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            AppendGameObjectTree(sb, transform.GetChild(i), depth + 1);
        }
    }

    private static void AppendImportantSceneObjects(StringBuilder sb, List<SceneObjectInfo> sceneObjects)
    {
        sb.AppendLine("## Important Scene Objects / Inspector Snapshot");
        sb.AppendLine();

        foreach (SceneObjectInfo info in sceneObjects)
        {
            bool importantByName = ImportantObjectNameKeywords.Any(k =>
                info.name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0 ||
                info.path.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);

            bool importantByComponent = info.componentNames.Any(c =>
                c.EndsWith("Manager", StringComparison.OrdinalIgnoreCase) ||
                c.EndsWith("UI", StringComparison.OrdinalIgnoreCase) ||
                c.Contains("Spawner", StringComparison.OrdinalIgnoreCase) ||
                c.Contains("Health", StringComparison.OrdinalIgnoreCase) ||
                c.Contains("Controller", StringComparison.OrdinalIgnoreCase));

            if (!importantByName && !importantByComponent)
                continue;

            sb.AppendLine($"### {info.path}");
            sb.AppendLine($"- Active: {info.activeSelf}");
            sb.AppendLine($"- Tag: {info.tag}");
            sb.AppendLine($"- Layer: {LayerMask.LayerToName(info.layer)}");
            sb.AppendLine($"- Components: {string.Join(", ", info.componentNames)}");
            sb.AppendLine();

            foreach (Component component in info.components)
            {
                if (component == null)
                    continue;

                Type type = component.GetType();

                if (type == typeof(Transform) || type == typeof(RectTransform))
                {
                    AppendTransformInfo(sb, component);
                    continue;
                }

                if (ShouldSkipVerboseBuiltInComponent(type))
                    continue;

                sb.AppendLine($"#### {type.Name}");
                AppendSerializedObjectFields(sb, component);
                sb.AppendLine();
            }
        }
    }

    private static void AppendUiStructure(StringBuilder sb, List<SceneObjectInfo> sceneObjects)
    {
        sb.AppendLine("## UI Structure Analysis");
        sb.AppendLine();

        List<SceneObjectInfo> uiObjects = sceneObjects
            .Where(o =>
                o.componentNames.Contains("Canvas") ||
                o.componentNames.Contains("Button") ||
                o.componentNames.Contains("TextMeshProUGUI") ||
                o.componentNames.Contains("Slider") ||
                o.componentNames.Contains("Image") ||
                o.name.Contains("Canvas") ||
                o.name.Contains("Panel") ||
                o.name.Contains("Text") ||
                o.name.Contains("Button"))
            .OrderBy(o => o.path)
            .ToList();

        if (uiObjects.Count == 0)
        {
            sb.AppendLine("- UI 오브젝트 없음");
            sb.AppendLine();
            return;
        }

        sb.AppendLine("```text");

        foreach (SceneObjectInfo info in uiObjects)
        {
            sb.AppendLine($"- {info.path} [{string.Join(", ", info.componentNames)}]");
        }

        sb.AppendLine("```");
        sb.AppendLine();

        sb.AppendLine("### UI Warnings");
        sb.AppendLine();

        bool hasHudCanvas = sceneObjects.Any(o => o.name == "HUDCanvas");
        bool hasHudPanel = sceneObjects.Any(o => o.name == "HUDPanel");
        bool hasTimerText = sceneObjects.Any(o => o.name == "TimerText");
        bool hasRelicSelectPanel = sceneObjects.Any(o => o.name == "RelicSelectPanel");

        sb.AppendLine($"- HUDCanvas exists: {hasHudCanvas}");
        sb.AppendLine($"- HUDPanel exists: {hasHudPanel}");
        sb.AppendLine($"- TimerText exists: {hasTimerText}");
        sb.AppendLine($"- RelicSelectPanel exists: {hasRelicSelectPanel}");
        sb.AppendLine();

        if (hasHudCanvas && hasHudPanel)
        {
            sb.AppendLine("- HUD 관련 신규 작업은 기존 HUDCanvas/HUDPanel 구조를 우선 검토해야 함.");
        }

        if (hasRelicSelectPanel)
        {
            sb.AppendLine("- 유물 선택 UI는 Canvas 기반 구조가 이미 존재함. OnGUI DebugUI 기준으로 작업하지 말 것.");
        }

        sb.AppendLine();
    }

    private static void AppendScriptableObjects(StringBuilder sb)
    {
        sb.AppendLine("## ScriptableObjects Snapshot");
        sb.AppendLine();

        string folder = ProjectRoot + "/ScriptableObjects";

        if (!Directory.Exists(folder))
        {
            sb.AppendLine("- ScriptableObjects 폴더 없음");
            sb.AppendLine();
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folder });

        if (guids.Length == 0)
        {
            sb.AppendLine("- ScriptableObject 없음");
            sb.AppendLine();
            return;
        }

        foreach (string guid in guids.OrderBy(g => AssetDatabase.GUIDToAssetPath(g)))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (asset == null)
                continue;

            sb.AppendLine($"### {path}");
            sb.AppendLine($"- Type: {asset.GetType().Name}");
            AppendSerializedObjectFields(sb, asset);
            sb.AppendLine();
        }
    }

    private static void AppendPrefabSummary(StringBuilder sb)
    {
        sb.AppendLine("## Prefabs Snapshot");
        sb.AppendLine();

        string prefabFolder = ProjectRoot + "/Prefabs";

        if (!Directory.Exists(prefabFolder))
        {
            sb.AppendLine("- Prefabs 폴더 없음");
            sb.AppendLine();
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolder });

        if (guids.Length == 0)
        {
            sb.AppendLine("- Prefab 없음");
            sb.AppendLine();
            return;
        }

        foreach (string guid in guids.OrderBy(g => AssetDatabase.GUIDToAssetPath(g)))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                continue;

            sb.AppendLine($"### {path}");
            sb.AppendLine("```text");
            AppendPrefabTree(sb, prefab.transform, 0);
            sb.AppendLine("```");
            sb.AppendLine();
        }
    }

    private static void AppendPrefabTree(StringBuilder sb, Transform transform, int depth)
    {
        if (transform == null)
            return;

        string indent = new string(' ', depth * 2);
        string components = string.Join(", ", transform.gameObject.GetComponents<Component>()
            .Where(c => c != null)
            .Select(c => c.GetType().Name));

        sb.AppendLine($"{indent}- {transform.gameObject.name} [{components}]");

        Component[] comps = transform.gameObject.GetComponents<Component>();

        foreach (Component comp in comps)
        {
            if (comp == null)
            {
                sb.AppendLine($"{indent}  ! Missing Script");
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            AppendPrefabTree(sb, transform.GetChild(i), depth + 1);
        }
    }

    private static void AppendFileChangeSummary(
        StringBuilder sb,
        List<FileSnapshot> currentFiles,
        List<FileSnapshot> addedFiles,
        List<FileSnapshot> modifiedFiles,
        List<FileSnapshot> deletedFiles)
    {
        sb.AppendLine("## File Change Summary");
        sb.AppendLine();
        sb.AppendLine($"- Added: {addedFiles.Count}");
        sb.AppendLine($"- Modified: {modifiedFiles.Count}");
        sb.AppendLine($"- Deleted: {deletedFiles.Count}");
        sb.AppendLine($"- Total tracked files: {currentFiles.Count}");
        sb.AppendLine();

        if (addedFiles.Count == 0 && modifiedFiles.Count == 0 && deletedFiles.Count == 0)
        {
            sb.AppendLine("- 변경된 파일 없음");
            sb.AppendLine();
            return;
        }

        AppendFileList(sb, "Added", addedFiles);
        AppendFileList(sb, "Modified", modifiedFiles);
        AppendFileList(sb, "Deleted", deletedFiles);
    }

    private static void AppendCodeChangePreview(
        StringBuilder sb,
        Dictionary<string, FileSnapshot> previousMap,
        List<FileSnapshot> addedFiles,
        List<FileSnapshot> modifiedFiles)
    {
        sb.AppendLine("## Code Change Preview");
        sb.AppendLine();

        List<FileSnapshot> changedCs = addedFiles.Concat(modifiedFiles)
            .Where(f => f.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (changedCs.Count == 0)
        {
            sb.AppendLine("- 변경된 C# 코드 없음");
            sb.AppendLine();
            return;
        }

        foreach (FileSnapshot file in changedCs)
        {
            previousMap.TryGetValue(file.path, out FileSnapshot previous);

            sb.AppendLine($"### {file.path}");
            sb.AppendLine();

            AppendCodeStructure(sb, file.text);
            AppendSimpleDiff(sb, previous?.text ?? string.Empty, file.text);
            sb.AppendLine();
        }
    }

    private static void AppendScriptsTree(StringBuilder sb, List<FileSnapshot> currentFiles)
    {
        sb.AppendLine("## Scripts Tree");
        sb.AppendLine();
        sb.AppendLine("```text");

        foreach (FileSnapshot file in currentFiles)
        {
            if (file.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine("- " + file.path);
            }
        }

        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendCurrentScriptStructure(StringBuilder sb, List<FileSnapshot> currentFiles)
    {
        sb.AppendLine("## Current Script Structure");
        sb.AppendLine();

        foreach (FileSnapshot file in currentFiles)
        {
            if (!file.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                continue;

            sb.AppendLine($"### {file.path}");
            AppendCodeStructure(sb, file.text);
            sb.AppendLine();
        }
    }

    private static void AppendFullSourceCode(StringBuilder sb, List<FileSnapshot> currentFiles)
    {
        sb.AppendLine("## Full Source Code");
        sb.AppendLine();

        foreach (FileSnapshot file in currentFiles)
        {
            if (!file.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                continue;

            sb.AppendLine($"### FILE: {file.path}");
            sb.AppendLine();
            sb.AppendLine("```csharp");
            sb.AppendLine(file.text);
            sb.AppendLine("```");
            sb.AppendLine();
        }
    }

    private static List<DiagnosticIssue> BuildDiagnostics(List<FileSnapshot> currentFiles, List<SceneObjectInfo> sceneObjects)
    {
        List<DiagnosticIssue> issues = new List<DiagnosticIssue>();

        DetectMissingScripts(sceneObjects, issues);
        DetectDuplicateImportantObjects(sceneObjects, issues);
        DetectEmptyImportantObjects(sceneObjects, issues);
        DetectDebugAndOfficialCoexistence(currentFiles, sceneObjects, issues);
        DetectSerializedReferenceProblems(sceneObjects, issues);
        DetectScriptableObjectProblems(issues);
        DetectPrefabProblems(issues);
        DetectCodeSmells(currentFiles, issues);
        DetectKnownProjectRisks(currentFiles, sceneObjects, issues);

        return issues;
    }

    private static void DetectMissingScripts(List<SceneObjectInfo> sceneObjects, List<DiagnosticIssue> issues)
    {
        foreach (SceneObjectInfo info in sceneObjects)
        {
            if (info.components.Any(c => c == null))
            {
                issues.Add(new DiagnosticIssue(
                    "HIGH",
                    "Missing Script in Scene",
                    $"{info.path}에 Missing Script가 존재함.",
                    "해당 컴포넌트를 제거하거나 올바른 스크립트를 복구한다."
                ));
            }
        }
    }

    private static void DetectDuplicateImportantObjects(List<SceneObjectInfo> sceneObjects, List<DiagnosticIssue> issues)
    {
        var groups = sceneObjects
            .GroupBy(o => o.name)
            .Where(g => g.Count() > 1)
            .OrderByDescending(g => g.Count());

        foreach (var group in groups)
        {
            string name = group.Key;

            bool important = ImportantObjectNameKeywords.Any(k => name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0) ||
                             name.IndexOf("Grid", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             name.IndexOf("Canvas", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             name.IndexOf("Manager", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             name.IndexOf("UI", StringComparison.OrdinalIgnoreCase) >= 0;

            if (!important)
                continue;

            issues.Add(new DiagnosticIssue(
                "MEDIUM",
                "Duplicate Important GameObject Name",
                $"{name} 오브젝트가 {group.Count()}개 존재함: {string.Join(", ", group.Select(o => o.path))}",
                "실제로 필요한 중복인지 확인하고, 테스트용/빈 오브젝트는 삭제 또는 명확히 이름 변경한다."
            ));
        }
    }

    private static void DetectEmptyImportantObjects(List<SceneObjectInfo> sceneObjects, List<DiagnosticIssue> issues)
    {
        foreach (SceneObjectInfo info in sceneObjects)
        {
            bool onlyTransform = info.componentNames.All(c => c == "Transform" || c == "RectTransform");
            bool noChildren = info.gameObject.transform.childCount == 0;

            bool importantName = info.name.IndexOf("HUD", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                 info.name.IndexOf("Grid", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                 info.name.IndexOf("UI", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                 info.name.IndexOf("Manager", StringComparison.OrdinalIgnoreCase) >= 0;

            if (onlyTransform && noChildren && importantName)
            {
                issues.Add(new DiagnosticIssue(
                    "LOW",
                    "Empty Important-Looking GameObject",
                    $"{info.path}는 이름은 중요해 보이지만 Transform만 있고 자식도 없음.",
                    "실제 사용 중인지 확인하고, 필요 없으면 삭제하거나 테스트용 이름으로 변경한다."
                ));
            }
        }
    }

    private static void DetectDebugAndOfficialCoexistence(List<FileSnapshot> currentFiles, List<SceneObjectInfo> sceneObjects, List<DiagnosticIssue> issues)
    {
        bool hasHudDebug = currentFiles.Any(f => f.path.EndsWith("HUDDebugUI.cs", StringComparison.OrdinalIgnoreCase));
        bool hasHudOfficial = currentFiles.Any(f => f.path.EndsWith("HUDCanvasUI.cs", StringComparison.OrdinalIgnoreCase));
        bool hasRelicDebug = currentFiles.Any(f => f.path.EndsWith("RelicSelectDebugUI.cs", StringComparison.OrdinalIgnoreCase));
        bool hasRelicOfficial = currentFiles.Any(f => f.path.EndsWith("RelicSelectUI.cs", StringComparison.OrdinalIgnoreCase));

        if (hasHudDebug && hasHudOfficial)
        {
            issues.Add(new DiagnosticIssue(
                "MEDIUM",
                "HUD DebugUI and Official HUD UI Coexist",
                "HUDDebugUI.cs와 HUDCanvasUI.cs가 동시에 존재함.",
                "신규 HUD 작업은 HUDCanvasUI 기준으로 진행하고, HUDDebugUI는 디버그/보류 후보로 분류한다."
            ));
        }

        if (hasRelicDebug && hasRelicOfficial)
        {
            issues.Add(new DiagnosticIssue(
                "MEDIUM",
                "Relic DebugUI and Official Relic UI Coexist",
                "RelicSelectDebugUI.cs와 RelicSelectUI.cs가 동시에 존재함.",
                "신규 유물 UI 작업은 RelicSelectUI 기준으로만 진행한다."
            ));
        }

        foreach (FileSnapshot file in currentFiles.Where(f => f.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)))
        {
            if (file.text.Contains("OnGUI("))
            {
                issues.Add(new DiagnosticIssue(
                    "LOW",
                    "OnGUI Usage Detected",
                    $"{file.path}에서 OnGUI 사용 감지.",
                    "OnGUI는 테스트/디버그용으로만 유지하고 출시형 UI는 Canvas 기반으로 진행한다."
                ));
            }
        }
    }

    private static void DetectSerializedReferenceProblems(List<SceneObjectInfo> sceneObjects, List<DiagnosticIssue> issues)
    {
        foreach (SceneObjectInfo info in sceneObjects)
        {
            foreach (Component component in info.components)
            {
                if (component == null)
                    continue;

                Type type = component.GetType();

                if (type == typeof(Transform) || type == typeof(RectTransform))
                    continue;

                if (ShouldSkipVerboseBuiltInComponent(type))
                    continue;

                List<SerializedFieldLine> fields = GetSerializedFieldLines(component);

                foreach (SerializedFieldLine field in fields)
                {
                    if (!field.isNullObjectReference)
                        continue;

                    if (field.isOptionalReference)
                        continue;

                    string severity = IsCriticalComponent(type.Name) ? "HIGH" : "MEDIUM";

                    issues.Add(new DiagnosticIssue(
                        severity,
                        "Possible Missing Inspector Reference",
                        $"{info.path} / {type.Name}.{field.path} = None",
                        "이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다."
                    ));
                }
            }
        }
    }

    private static void DetectScriptableObjectProblems(List<DiagnosticIssue> issues)
    {
        string folder = ProjectRoot + "/ScriptableObjects";

        if (!Directory.Exists(folder))
            return;

        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folder });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (asset == null)
                continue;

            List<SerializedFieldLine> fields = GetSerializedFieldLines(asset);

            foreach (SerializedFieldLine field in fields)
            {
                if (!field.isNullObjectReference)
                    continue;

                if (field.isOptionalReference)
                    continue;

                issues.Add(new DiagnosticIssue(
                    "LOW",
                    "ScriptableObject Null Reference",
                    $"{path} / {field.path} = None",
                    "아이콘 같은 선택 필드면 무시 가능. 필수 데이터라면 연결한다."
                ));
            }
        }
    }

    private static void DetectPrefabProblems(List<DiagnosticIssue> issues)
    {
        string prefabFolder = ProjectRoot + "/Prefabs";

        if (!Directory.Exists(prefabFolder))
            return;

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolder });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                continue;

            List<GameObject> objects = new List<GameObject>();
            CollectGameObjects(prefab.transform, objects);

            foreach (GameObject go in objects)
            {
                Component[] comps = go.GetComponents<Component>();

                if (comps.Any(c => c == null))
                {
                    issues.Add(new DiagnosticIssue(
                        "HIGH",
                        "Missing Script in Prefab",
                        $"{path} / {GetRelativePrefabPath(prefab.transform, go.transform)}에 Missing Script 존재.",
                        "Prefab을 열어 Missing Script를 제거하거나 복구한다."
                    ));
                }
            }
        }
    }

    private static void DetectCodeSmells(List<FileSnapshot> currentFiles, List<DiagnosticIssue> issues)
    {
        foreach (FileSnapshot file in currentFiles.Where(f => f.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)))
        {
            if (file.text.Contains("FindFirstObjectByType<"))
            {
                issues.Add(new DiagnosticIssue(
                    "LOW",
                    "FindFirstObjectByType Usage",
                    $"{file.path}에서 FindFirstObjectByType 사용 감지.",
                    "초기 단계에서는 허용 가능. 호출 빈도가 높거나 확장되면 Inspector 참조 또는 이벤트 구조로 교체 검토."
                ));
            }

            if (file.text.Contains("Time.timeScale = 0f") && file.text.Contains("Time.timeScale = 1f"))
            {
                issues.Add(new DiagnosticIssue(
                    "LOW",
                    "Direct TimeScale Control",
                    $"{file.path}에서 Time.timeScale 직접 제어 감지.",
                    "여러 UI가 동시에 pause를 제어하면 충돌 가능. 나중에 PauseManager로 통합 검토."
                ));
            }

            if (file.text.Contains("Destroy(gameObject)") && file.path.Contains("/Enemy/"))
            {
                issues.Add(new DiagnosticIssue(
                    "LOW",
                    "Destroy Enemy Object",
                    $"{file.path}에서 Destroy(gameObject) 사용.",
                    "5분 MVP에서는 허용. 적 수가 많아지면 Object Pooling 검토."
                ));
            }
        }
    }

    private static void DetectKnownProjectRisks(List<FileSnapshot> currentFiles, List<SceneObjectInfo> sceneObjects, List<DiagnosticIssue> issues)
    {
        bool hasHudCanvas = sceneObjects.Any(o => o.name == "HUDCanvas");
        bool hasHudPanel = sceneObjects.Any(o => o.name == "HUDPanel");
        bool hasHudCanvasUi = currentFiles.Any(f => f.path.EndsWith("HUDCanvasUI.cs", StringComparison.OrdinalIgnoreCase));

        if (hasHudCanvas && hasHudPanel && hasHudCanvasUi)
        {
            bool hasRelicDisplayScript = currentFiles.Any(f => f.path.EndsWith("HUDRelicDisplay.cs", StringComparison.OrdinalIgnoreCase));
            bool hasRelicListPanel = sceneObjects.Any(o => o.name.IndexOf("RelicList", StringComparison.OrdinalIgnoreCase) >= 0);

            if (!hasRelicListPanel)
            {
                issues.Add(new DiagnosticIssue(
                    "MEDIUM",
                    "No Persistent Relic HUD Display",
                    "유물 선택 UI는 있으나 HUD에 보유 유물을 지속 표시하는 구조가 감지되지 않음.",
                    "새 UI를 만들기 전 HUDCanvasUI와 기존 HUDPanel 구조를 먼저 확인하고 통합 방식을 결정한다."
                ));
            }

            if (hasRelicDisplayScript && !hasRelicListPanel)
            {
                issues.Add(new DiagnosticIssue(
                    "MEDIUM",
                    "Relic Display Script Without Scene UI",
                    "HUDRelicDisplay.cs는 있으나 RelicList 계열 UI 오브젝트가 감지되지 않음.",
                    "스크립트만 추가된 상태인지 확인하고, 기존 HUD 구조와 연결 여부를 점검한다."
                ));
            }
        }

        FileSnapshot enemyHealth = currentFiles.FirstOrDefault(f => f.path.EndsWith("EnemyHealth.cs", StringComparison.OrdinalIgnoreCase));

        if (enemyHealth != null &&
            enemyHealth.text.Contains("DropExpGem") &&
            enemyHealth.text.Contains("DropSmallHeal") &&
            CountOccurrences(enemyHealth.text, "transform.position") >= 2)
        {
            issues.Add(new DiagnosticIssue(
                "MEDIUM",
                "Drop Items May Overlap",
                "EnemyHealth에서 ExpGem과 SmallHeal이 같은 transform.position으로 생성될 가능성이 있음.",
                "SmallHeal 드랍 위치를 약간 오프셋하여 ExpGem과 겹치지 않게 한다."
            ));
        }

        int weaponScripts = currentFiles.Count(f =>
            f.path.Contains("/Weapons/", StringComparison.OrdinalIgnoreCase) &&
            f.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase));

        if (weaponScripts <= 1)
        {
            issues.Add(new DiagnosticIssue(
                "MEDIUM",
                "Only One Weapon Script Detected",
                "Weapons 폴더에 무기 스크립트가 1개 이하로 감지됨.",
                "근접 무기 단조로움 해소를 위해 두 번째 무기 구조를 계획한다. 단, HUD/드랍/웨이브 정리 후 진행."
            ));
        }
    }

    private static List<SceneObjectInfo> CollectSceneObjectInfos()
    {
        List<SceneObjectInfo> result = new List<SceneObjectInfo>();

        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] roots = activeScene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            CollectSceneObjectInfoRecursive(root.transform, result);
        }

        return result;
    }

    private static void CollectSceneObjectInfoRecursive(Transform transform, List<SceneObjectInfo> result)
    {
        if (transform == null)
            return;

        GameObject go = transform.gameObject;
        Component[] components = go.GetComponents<Component>();

        SceneObjectInfo info = new SceneObjectInfo
        {
            gameObject = go,
            path = GetGameObjectPath(go),
            name = go.name,
            activeSelf = go.activeSelf,
            tag = go.tag,
            layer = go.layer,
            components = components.ToList(),
            componentNames = components.Select(c => c == null ? "Missing Script" : c.GetType().Name).ToList()
        };

        result.Add(info);

        for (int i = 0; i < transform.childCount; i++)
        {
            CollectSceneObjectInfoRecursive(transform.GetChild(i), result);
        }
    }

    private static void AppendTransformInfo(StringBuilder sb, Component component)
    {
        if (component == null)
            return;

        Transform transform = component as Transform;

        if (transform == null)
            return;

        sb.AppendLine($"#### {component.GetType().Name}");
        sb.AppendLine("```text");
        sb.AppendLine($"Local Position: {transform.localPosition}");
        sb.AppendLine($"Local Rotation: {transform.localEulerAngles}");
        sb.AppendLine($"Local Scale: {transform.localScale}");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendSerializedObjectFields(StringBuilder sb, UnityEngine.Object target)
    {
        List<SerializedFieldLine> fields = GetSerializedFieldLines(target);

        sb.AppendLine("```text");

        if (fields.Count == 0)
        {
            sb.AppendLine("No visible serialized fields");
        }
        else
        {
            int count = 0;

            foreach (SerializedFieldLine field in fields)
            {
                string indent = new string(' ', field.depth * 2);
                sb.AppendLine($"{indent}{field.path}: {field.value}");

                count++;

                if (count >= MaxSerializedPropertiesPerComponent)
                {
                    sb.AppendLine("# ... serialized fields truncated");
                    break;
                }
            }
        }

        sb.AppendLine("```");
    }

    private static List<SerializedFieldLine> GetSerializedFieldLines(UnityEngine.Object target)
    {
        List<SerializedFieldLine> result = new List<SerializedFieldLine>();

        if (target == null)
            return result;

        try
        {
            SerializedObject so = new SerializedObject(target);
            SerializedProperty iterator = so.GetIterator();

            bool enterChildren = true;
            int count = 0;

            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (iterator.name == "m_Script")
                    continue;

                if (iterator.propertyPath.StartsWith("m_"))
                    continue;

                if (iterator.depth > 4)
                    continue;

                string value = GetSerializedPropertyValue(iterator);

                if (string.IsNullOrEmpty(value))
                    continue;

                SerializedFieldLine line = new SerializedFieldLine
                {
                    path = iterator.propertyPath,
                    value = value,
                    depth = iterator.depth,
                    type = iterator.propertyType,
                    isNullObjectReference = iterator.propertyType == SerializedPropertyType.ObjectReference &&
                                            iterator.objectReferenceValue == null,
                    isOptionalReference = IsOptionalReferenceField(iterator.propertyPath)
                };

                result.Add(line);
                count++;

                if (count >= MaxSerializedPropertiesPerComponent)
                    break;
            }
        }
        catch (Exception ex)
        {
            result.Add(new SerializedFieldLine
            {
                path = "SerializationError",
                value = ex.Message,
                depth = 0,
                type = SerializedPropertyType.String,
                isNullObjectReference = false,
                isOptionalReference = false
            });
        }

        return result;
    }

    private static string GetSerializedPropertyValue(SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                return property.intValue.ToString();

            case SerializedPropertyType.Boolean:
                return property.boolValue.ToString();

            case SerializedPropertyType.Float:
                return property.floatValue.ToString("0.###");

            case SerializedPropertyType.String:
                return string.IsNullOrEmpty(property.stringValue) ? "\"\"" : property.stringValue;

            case SerializedPropertyType.Color:
                return property.colorValue.ToString();

            case SerializedPropertyType.ObjectReference:
                if (property.objectReferenceValue == null)
                    return "None";

                return $"{property.objectReferenceValue.name} ({property.objectReferenceValue.GetType().Name})";

            case SerializedPropertyType.LayerMask:
                return property.intValue.ToString();

            case SerializedPropertyType.Enum:
                if (property.enumDisplayNames != null &&
                    property.enumValueIndex >= 0 &&
                    property.enumValueIndex < property.enumDisplayNames.Length)
                {
                    return property.enumDisplayNames[property.enumValueIndex];
                }

                return property.enumValueIndex.ToString();

            case SerializedPropertyType.Vector2:
                return property.vector2Value.ToString();

            case SerializedPropertyType.Vector3:
                return property.vector3Value.ToString();

            case SerializedPropertyType.Vector4:
                return property.vector4Value.ToString();

            case SerializedPropertyType.Rect:
                return property.rectValue.ToString();

            case SerializedPropertyType.ArraySize:
                return property.intValue.ToString();

            case SerializedPropertyType.Character:
                return property.intValue.ToString();

            case SerializedPropertyType.AnimationCurve:
                return "AnimationCurve";

            case SerializedPropertyType.Bounds:
                return property.boundsValue.ToString();

            case SerializedPropertyType.Quaternion:
                return property.quaternionValue.eulerAngles.ToString();

            case SerializedPropertyType.ExposedReference:
                return "ExposedReference";

            case SerializedPropertyType.FixedBufferSize:
                return property.fixedBufferSize.ToString();

            case SerializedPropertyType.Vector2Int:
                return property.vector2IntValue.ToString();

            case SerializedPropertyType.Vector3Int:
                return property.vector3IntValue.ToString();

            case SerializedPropertyType.RectInt:
                return property.rectIntValue.ToString();

            case SerializedPropertyType.BoundsInt:
                return property.boundsIntValue.ToString();

            case SerializedPropertyType.ManagedReference:
                return string.IsNullOrEmpty(property.managedReferenceFullTypename)
                    ? "ManagedReference"
                    : property.managedReferenceFullTypename;

            case SerializedPropertyType.Generic:
                if (property.isArray)
                    return $"Array/List Size: {property.arraySize}";

                return string.Empty;

            default:
                return property.propertyType.ToString();
        }
    }

    private static bool ShouldSkipVerboseBuiltInComponent(Type type)
    {
        if (type == typeof(Canvas) ||
            type.Name == "CanvasScaler" ||
            type.Name == "GraphicRaycaster" ||
            type.Name == "CanvasRenderer" ||
            type.Name == "SpriteRenderer" ||
            type.Name == "Image" ||
            type.Name == "Button" ||
            type.Name == "TextMeshProUGUI" ||
            type.Name == "Slider")
        {
            return false;
        }

        return false;
    }

    private static bool IsCriticalComponent(string componentName)
    {
        return componentName.Contains("Manager") ||
               componentName.Contains("Spawner") ||
               componentName.EndsWith("UI") ||
               componentName.Contains("Health") ||
               componentName.Contains("Controller");
    }

    private static bool IsOptionalReferenceField(string propertyPath)
    {
        string lower = propertyPath.ToLowerInvariant();

        foreach (string keyword in OptionalReferenceKeywords)
        {
            if (lower.Contains(keyword.ToLowerInvariant()))
                return true;
        }

        return false;
    }

    private static string GetGameObjectPath(GameObject go)
    {
        if (go == null)
            return "None";

        Stack<string> names = new Stack<string>();
        Transform current = go.transform;

        while (current != null)
        {
            names.Push(current.name);
            current = current.parent;
        }

        return string.Join("/", names);
    }

    private static string GetRelativePrefabPath(Transform root, Transform target)
    {
        if (root == null || target == null)
            return "Unknown";

        Stack<string> names = new Stack<string>();
        Transform current = target;

        while (current != null && current != root.parent)
        {
            names.Push(current.name);
            if (current == root)
                break;
            current = current.parent;
        }

        return string.Join("/", names);
    }

    private static void CollectGameObjects(Transform transform, List<GameObject> list)
    {
        if (transform == null)
            return;

        list.Add(transform.gameObject);

        for (int i = 0; i < transform.childCount; i++)
        {
            CollectGameObjects(transform.GetChild(i), list);
        }
    }

    private static void AppendFileList(StringBuilder sb, string title, List<FileSnapshot> files)
    {
        if (files.Count == 0)
            return;

        sb.AppendLine($"### {title}");
        sb.AppendLine();

        foreach (FileSnapshot file in files)
        {
            sb.AppendLine($"- {file.path}");
        }

        sb.AppendLine();
    }

    private static void AppendCodeStructure(StringBuilder sb, string text)
    {
        List<string> enums = Regex.Matches(text, @"\benum\s+([A-Za-z_][A-Za-z0-9_]*)")
            .Cast<Match>()
            .Select(m => m.Groups[1].Value)
            .Distinct()
            .ToList();

        List<string> classes = Regex.Matches(text, @"\bclass\s+([A-Za-z_][A-Za-z0-9_]*)")
            .Cast<Match>()
            .Select(m => m.Groups[1].Value)
            .Distinct()
            .ToList();

        List<string> methods = Regex.Matches(
                text,
                @"\b(public|private|protected|internal)\s+(static\s+)?[A-Za-z0-9_<>\[\],\s]+\s+([A-Za-z_][A-Za-z0-9_]*)\s*\("
            )
            .Cast<Match>()
            .Select(m => m.Groups[3].Value)
            .Where(name => name != "if" && name != "for" && name != "while" && name != "switch")
            .Distinct()
            .ToList();

        sb.AppendLine("```text");

        if (enums.Count > 0)
        {
            sb.AppendLine("Enums:");
            foreach (string enumName in enums)
            {
                sb.AppendLine($"- {enumName}");
            }
        }

        if (classes.Count > 0)
        {
            sb.AppendLine("Classes:");
            foreach (string className in classes)
            {
                sb.AppendLine($"- {className}");
            }
        }

        if (methods.Count > 0)
        {
            sb.AppendLine("Methods:");
            foreach (string methodName in methods)
            {
                sb.AppendLine($"- {methodName}()");
            }
        }

        if (enums.Count == 0 && classes.Count == 0 && methods.Count == 0)
        {
            sb.AppendLine("No structure detected");
        }

        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendSimpleDiff(StringBuilder sb, string oldText, string newText)
    {
        string[] oldLines = SplitLines(oldText);
        string[] newLines = SplitLines(newText);

        List<string> changes = new List<string>();

        int max = Math.Max(oldLines.Length, newLines.Length);

        for (int i = 0; i < max; i++)
        {
            string oldLine = i < oldLines.Length ? oldLines[i] : null;
            string newLine = i < newLines.Length ? newLines[i] : null;

            if (oldLine == newLine)
                continue;

            if (oldLine != null)
            {
                changes.Add($"- {oldLine}");
            }

            if (newLine != null)
            {
                changes.Add($"+ {newLine}");
            }

            if (changes.Count >= MaxDiffLines)
                break;
        }

        sb.AppendLine("Changed code preview:");
        sb.AppendLine("```diff");

        if (changes.Count == 0)
        {
            sb.AppendLine("# 코드 변경 미리보기 없음");
        }
        else
        {
            foreach (string line in changes)
            {
                sb.AppendLine(line);
            }

            if (changes.Count >= MaxDiffLines)
            {
                sb.AppendLine("# ... 변경 내용 일부만 표시됨");
            }
        }

        sb.AppendLine("```");
    }

    private static int CountOccurrences(string text, string pattern)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
            return 0;

        int count = 0;
        int index = 0;

        while ((index = text.IndexOf(pattern, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += pattern.Length;
        }

        return count;
    }

    private static List<FileSnapshot> ScanProjectFiles()
    {
        List<FileSnapshot> result = new List<FileSnapshot>();

        if (!Directory.Exists(ProjectRoot))
        {
            Debug.LogWarning($"프로젝트 루트를 찾을 수 없습니다: {ProjectRoot}");
            return result;
        }

        string[] files = Directory.GetFiles(ProjectRoot, "*.*", SearchOption.AllDirectories);

        foreach (string rawPath in files)
        {
            string path = NormalizePath(rawPath);

            if (path.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
                continue;

            if (GeneratedOutputPaths.Contains(path))
                continue;

            string extension = Path.GetExtension(path).ToLowerInvariant();

            if (!TargetExtensions.Contains(extension))
                continue;

            byte[] bytes = File.ReadAllBytes(path);

            FileSnapshot snapshot = new FileSnapshot
            {
                path = path,
                hash = GetSha256(bytes),
                size = bytes.LongLength,
                text = ShouldStoreText(extension) ? SafeReadText(path) : string.Empty
            };

            result.Add(snapshot);
        }

        return result.OrderBy(f => f.path).ToList();
    }

    private static bool ShouldStoreText(string extension)
    {
        return extension == ".cs";
    }

    private static string SafeReadText(string path)
    {
        try
        {
            return File.ReadAllText(path, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            return $"// Failed to read file: {path}\n// {ex.Message}";
        }
    }

    private static SnapshotCache LoadCache()
    {
        if (!File.Exists(CachePath))
        {
            return new SnapshotCache();
        }

        try
        {
            string json = File.ReadAllText(CachePath, Encoding.UTF8);
            SnapshotCache cache = JsonUtility.FromJson<SnapshotCache>(json);

            if (cache == null)
                return new SnapshotCache();

            if (cache.files == null)
                cache.files = new List<FileSnapshot>();

            return cache;
        }
        catch
        {
            return new SnapshotCache();
        }
    }

    private static void SaveCache(List<FileSnapshot> files)
    {
        SnapshotCache cache = new SnapshotCache
        {
            files = files
        };

        string json = JsonUtility.ToJson(cache, true);
        File.WriteAllText(CachePath, json, Encoding.UTF8);
    }

    private static void EnsureDocsFolder()
    {
        if (!Directory.Exists(DocsFolder))
        {
            Directory.CreateDirectory(DocsFolder);
        }
    }

    private static string[] SplitLines(string text)
    {
        if (string.IsNullOrEmpty(text))
            return Array.Empty<string>();

        return text.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
    }

    private static string GetSha256(byte[] bytes)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] hash = sha.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }

    private static string NormalizePath(string path)
    {
        return path.Replace("\\", "/");
    }

    private static string NormalizePathStatic(string path)
    {
        return path.Replace("\\", "/");
    }
}
```

### FILE: Assets/_Project/Scripts/Editor/EnemySpawnerEditor.cs

```csharp
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    private SerializedProperty playerTarget;
    private SerializedProperty waveManager;
    private SerializedProperty enemySpawnEntries;
    private SerializedProperty spawnInterval;
    private SerializedProperty spawnRadius;
    private SerializedProperty maxEnemies;

    private ReorderableList enemyList;

    private void OnEnable()
    {
        playerTarget = serializedObject.FindProperty("playerTarget");
        waveManager = serializedObject.FindProperty("waveManager");
        enemySpawnEntries = serializedObject.FindProperty("enemySpawnEntries");
        spawnInterval = serializedObject.FindProperty("spawnInterval");
        spawnRadius = serializedObject.FindProperty("spawnRadius");
        maxEnemies = serializedObject.FindProperty("maxEnemies");

        enemyList = new ReorderableList(serializedObject, enemySpawnEntries, true, true, true, true);

        enemyList.drawHeaderCallback = DrawHeader;
        enemyList.drawElementCallback = DrawElement;
        enemyList.elementHeight = EditorGUIUtility.singleLineHeight + 8f;

        enemyList.onAddCallback = AddEnemyEntry;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawReferences();
        EditorGUILayout.Space(8f);

        DrawEnemySpawnTable();
        EditorGUILayout.Space(8f);

        DrawSpawnSettings();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawReferences()
    {
        EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(playerTarget);
        EditorGUILayout.PropertyField(waveManager);
    }

    private void DrawEnemySpawnTable()
    {
        EditorGUILayout.LabelField("Enemy Spawn Table", EditorStyles.boldLabel);

        enemyList.DoLayoutList();

        float totalWeight = GetTotalWeight();

        EditorGUILayout.HelpBox(
            $"Total Weight: {totalWeight:0.##}\n" +
            "Weight 값이 높을수록 더 자주 스폰됩니다. 빨간 박스 Enemy는 이 목록에서 빼면 더 이상 나오지 않습니다.",
            MessageType.Info
        );

        if (totalWeight <= 0f)
        {
            EditorGUILayout.HelpBox(
                "사용 가능한 몬스터가 없습니다. Prefab이 들어가 있고 Weight가 0보다 큰 항목이 최소 1개 필요합니다.",
                MessageType.Warning
            );
        }
    }

    private void DrawSpawnSettings()
    {
        EditorGUILayout.LabelField("Spawn Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(spawnInterval);
        EditorGUILayout.PropertyField(spawnRadius);
        EditorGUILayout.PropertyField(maxEnemies);
    }

    private void DrawHeader(Rect rect)
    {
        float x = rect.x;

        EditorGUI.LabelField(new Rect(x, rect.y, 45f, rect.height), "Use");
        x += 45f;

        EditorGUI.LabelField(new Rect(x, rect.y, 120f, rect.height), "Name");
        x += 120f;

        EditorGUI.LabelField(new Rect(x, rect.y, rect.width - 300f, rect.height), "Prefab");

        EditorGUI.LabelField(new Rect(rect.xMax - 130f, rect.y, 70f, rect.height), "Weight");
        EditorGUI.LabelField(new Rect(rect.xMax - 55f, rect.y, 55f, rect.height), "Rate");
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = enemySpawnEntries.GetArrayElementAtIndex(index);

        SerializedProperty enabled = element.FindPropertyRelative("enabled");
        SerializedProperty displayName = element.FindPropertyRelative("displayName");
        SerializedProperty enemyPrefab = element.FindPropertyRelative("enemyPrefab");
        SerializedProperty spawnWeight = element.FindPropertyRelative("spawnWeight");

        rect.y += 4f;
        rect.height = EditorGUIUtility.singleLineHeight;

        float x = rect.x;

        EditorGUI.PropertyField(
            new Rect(x, rect.y, 35f, rect.height),
            enabled,
            GUIContent.none
        );
        x += 45f;

        EditorGUI.PropertyField(
            new Rect(x, rect.y, 110f, rect.height),
            displayName,
            GUIContent.none
        );
        x += 120f;

        EditorGUI.PropertyField(
            new Rect(x, rect.y, rect.width - 310f, rect.height),
            enemyPrefab,
            GUIContent.none
        );

        EditorGUI.PropertyField(
            new Rect(rect.xMax - 130f, rect.y, 65f, rect.height),
            spawnWeight,
            GUIContent.none
        );

        float totalWeight = GetTotalWeight();
        float rate = 0f;

        if (enabled.boolValue && enemyPrefab.objectReferenceValue != null && spawnWeight.floatValue > 0f && totalWeight > 0f)
        {
            rate = spawnWeight.floatValue / totalWeight * 100f;
        }

        EditorGUI.LabelField(
            new Rect(rect.xMax - 55f, rect.y, 55f, rect.height),
            $"{rate:0.#}%"
        );
    }

    private void AddEnemyEntry(ReorderableList list)
    {
        enemySpawnEntries.arraySize++;

        SerializedProperty element = enemySpawnEntries.GetArrayElementAtIndex(enemySpawnEntries.arraySize - 1);

        element.FindPropertyRelative("enabled").boolValue = true;
        element.FindPropertyRelative("displayName").stringValue = "New Enemy";
        element.FindPropertyRelative("enemyPrefab").objectReferenceValue = null;
        element.FindPropertyRelative("spawnWeight").floatValue = 1f;
    }

    private float GetTotalWeight()
    {
        float total = 0f;

        for (int i = 0; i < enemySpawnEntries.arraySize; i++)
        {
            SerializedProperty element = enemySpawnEntries.GetArrayElementAtIndex(i);

            bool enabled = element.FindPropertyRelative("enabled").boolValue;
            Object prefab = element.FindPropertyRelative("enemyPrefab").objectReferenceValue;
            float weight = element.FindPropertyRelative("spawnWeight").floatValue;

            if (!enabled)
                continue;

            if (prefab == null)
                continue;

            if (weight <= 0f)
                continue;

            total += weight;
        }

        return total;
    }
}
```

### FILE: Assets/_Project/Scripts/Enemy/EnemyContactDamage.cs

```csharp
using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageInterval = 0.8f;

    private float damageTimer;

    public void Initialize(int newDamage)
    {
        damage = Mathf.Max(1, newDamage);
        damageTimer = 0f;
    }

    private void Update()
    {
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (damageTimer > 0f)
            return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth == null)
            return;

        Vector2 hitDirection = other.transform.position - transform.position;

        playerHealth.TakeDamage(damage, hitDirection);

        damageTimer = damageInterval;
    }
}
```

### FILE: Assets/_Project/Scripts/Enemy/EnemyHealth.cs

```csharp
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 30;

    [Header("Reward")]
    [SerializeField] private GameObject expGemPrefab;
    [SerializeField] private int expReward = 5;

    [Header("Small Heal Drop")]
    [SerializeField] private GameObject smallHealPrefab;
    [SerializeField, Range(0f, 1f)] private float smallHealDropChance = 0.07f;
    [SerializeField] private bool midBossAlwaysDropSmallHeal = true;
    [SerializeField] private float smallHealDropOffsetMin = 0.35f;
    [SerializeField] private float smallHealDropOffsetMax = 0.75f;
    [SerializeField] private float expGemDropOffsetRadius = 0.15f;
    [SerializeField] private float minDistanceBetweenDrops = 0.3f;

    [Header("Boss Visual")]
    [SerializeField] private float normalScale = 0.8f;
    [SerializeField] private float midBossScale = 1.8f;

    [Tooltip("체크하면 중간보스에만 색상 강조를 적용합니다.")]
    [SerializeField] private bool useMidBossTint = false;

    [SerializeField] private Color midBossTintColor = new Color(1f, 0.65f, 0.12f, 1f);

    [Header("Hit Reaction")]
    [SerializeField] private Color hitFlashColor = new Color(1f, 0.35f, 0.25f, 1f);
    [SerializeField] private float hitFlashDuration = 0.12f;
    [SerializeField] private float hitScaleMultiplier = 1.12f;
    [SerializeField] private float knockbackForce = 3.5f;
    [SerializeField] private float knockbackDuration = 0.08f;

    private int currentHealth;
    private bool isDead;
    private bool isMidBoss;

    private SpriteRenderer spriteRenderer;
    private EnemyMovement enemyMovement;
    private Color originalColor;
    private Vector3 baseScale;
    private Coroutine hitFlashCoroutine;
    private EnemySpawner ownerSpawner;

    public bool IsMidBoss => isMidBoss;

    private void Awake()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyMovement = GetComponent<EnemyMovement>();

        baseScale = transform.localScale;

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void Initialize(int newMaxHealth, int newExpReward, bool isBoss, EnemySpawner spawner = null)
    {
        ownerSpawner = spawner;
        isMidBoss = isBoss;
        isDead = false;

        maxHealth = Mathf.Max(1, newMaxHealth);
        currentHealth = maxHealth;
        expReward = Mathf.Max(1, newExpReward);

        if (hitFlashCoroutine != null)
        {
            StopCoroutine(hitFlashCoroutine);
            hitFlashCoroutine = null;
        }

        ApplyVisual();
    }

    private void ApplyVisual()
    {
        if (isMidBoss)
        {
            gameObject.name = "MidBoss_Enemy";
            transform.localScale = Vector3.one * midBossScale;
            baseScale = transform.localScale;

            if (spriteRenderer != null)
            {
                if (useMidBossTint)
                {
                    spriteRenderer.color = midBossTintColor;
                    originalColor = midBossTintColor;
                }
                else
                {
                    spriteRenderer.color = Color.white;
                    originalColor = Color.white;
                }
            }

            return;
        }

        gameObject.name = "Enemy";
        transform.localScale = Vector3.one * normalScale;
        baseScale = transform.localScale;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
            originalColor = Color.white;
        }
    }

    public void TakeDamage(int damage)
    {
        TakeDamage(damage, transform.position);
    }

    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        if (isDead)
            return;

        if (damage <= 0)
            return;

        currentHealth -= damage;

        Vector2 knockbackDirection = (Vector2)transform.position - attackerPosition;

        if (enemyMovement != null)
        {
            float finalKnockbackForce = isMidBoss ? knockbackForce * 0.45f : knockbackForce;
            enemyMovement.ApplyKnockback(knockbackDirection, finalKnockbackForce, knockbackDuration);
        }

        PlayHitFlash();

        Debug.Log($"{gameObject.name} took {damage} damage. HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void PlayHitFlash()
    {
        if (hitFlashCoroutine != null)
        {
            StopCoroutine(hitFlashCoroutine);
        }

        hitFlashCoroutine = StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitFlashColor;
        }

        transform.localScale = baseScale * hitScaleMultiplier;

        yield return new WaitForSeconds(hitFlashDuration);

        if (!isDead)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }

            transform.localScale = baseScale;
        }

        hitFlashCoroutine = null;
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        NotifyEnemyKilled();

        Vector3 expGemDropPosition = GetExpGemDropPosition();
        DropExpGem(expGemDropPosition);
        DropSmallHeal(expGemDropPosition);

        if (isMidBoss)
        {
            OpenRelicSelectUI();
        }

        if (ownerSpawner != null)
        {
            ownerSpawner.DespawnEnemy(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void NotifyEnemyKilled()
    {
        PlayerRelicEffects relicEffects = FindFirstObjectByType<PlayerRelicEffects>();

        if (relicEffects == null)
            return;

        relicEffects.OnEnemyKilled();
    }

    private void DropExpGem(Vector3 dropPosition)
    {
        if (expGemPrefab == null)
            return;

        GameObject expGem = Instantiate(expGemPrefab, dropPosition, Quaternion.identity);

        ExpGem gem = expGem.GetComponent<ExpGem>();

        if (gem != null)
        {
            gem.SetExpAmount(expReward);
        }
    }

    private void DropSmallHeal(Vector3 expGemDropPosition)
    {
        if (smallHealPrefab == null)
            return;

        if (isMidBoss && midBossAlwaysDropSmallHeal)
        {
            Instantiate(smallHealPrefab, GetSmallHealDropPosition(expGemDropPosition), Quaternion.identity);
            return;
        }

        if (Random.value > smallHealDropChance)
            return;

        Instantiate(smallHealPrefab, GetSmallHealDropPosition(expGemDropPosition), Quaternion.identity);
    }

    private Vector3 GetExpGemDropPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle * expGemDropOffsetRadius;
        return transform.position + (Vector3)randomOffset;
    }

    private Vector3 GetSmallHealDropPosition(Vector3 expGemDropPosition)
    {
        Vector3 smallHealPosition = transform.position;
        int maxRetryCount = 5;

        for (int i = 0; i < maxRetryCount; i++)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            if (randomDirection.sqrMagnitude <= 0.01f)
            {
                randomDirection = Vector2.right;
            }

            float distance = Random.Range(smallHealDropOffsetMin, smallHealDropOffsetMax);
            smallHealPosition = transform.position + (Vector3)(randomDirection * distance);

            if ((smallHealPosition - expGemDropPosition).sqrMagnitude >= minDistanceBetweenDrops * minDistanceBetweenDrops)
            {
                break;
            }
        }

        return smallHealPosition;
    }

    private void OpenRelicSelectUI()
    {
        RelicSelectUI relicSelectUI = FindFirstObjectByType<RelicSelectUI>();

        if (relicSelectUI == null)
        {
            Debug.LogWarning("RelicSelectUI not found in scene.");
            return;
        }

        relicSelectUI.Open();
    }
}
```

### FILE: Assets/_Project/Scripts/Enemy/EnemyMovement.cs

```csharp
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    private static readonly List<EnemyMovement> ActiveEnemies = new List<EnemyMovement>();

    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Movement")]
    [SerializeField] private float baseMoveSpeed = 2.4f;
    [SerializeField] private float stopDistance = 0.45f;

    [Header("Swarm Target Offset")]
    [SerializeField] private float targetOffsetMinRadius = 0.25f;
    [SerializeField] private float targetOffsetMaxRadius = 1.05f;
    [SerializeField] private float offsetChangeInterval = 2.5f;

    [Header("Separation")]
    [SerializeField] private float separationRadius = 0.85f;
    [SerializeField] private float separationWeight = 1.15f;
    [SerializeField] private float maxSeparationForce = 1.4f;

    [Header("Near Player Behavior")]
    [SerializeField] private float nearPlayerDistance = 1.25f;
    [SerializeField] private float nearPlayerChaseWeight = 0.55f;

    private Rigidbody2D rb;

    private Vector2 personalTargetOffset;
    private float moveSpeed;
    private float offsetTimer;

    private Vector2 knockbackVelocity;
    private float knockbackTimer;

    private void OnEnable()
    {
        if (!ActiveEnemies.Contains(this))
        {
            ActiveEnemies.Add(this);
        }

        knockbackVelocity = Vector2.zero;
        knockbackTimer = 0f;
        PickNewTargetOffset();
        offsetTimer = Random.Range(0f, offsetChangeInterval);
    }

    private void OnDisable()
    {
        ActiveEnemies.Remove(this);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = baseMoveSpeed * Random.Range(0.9f, 1.12f);
        PickNewTargetOffset();
        offsetTimer = Random.Range(0f, offsetChangeInterval);
    }

    private void FixedUpdate()
    {
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = knockbackVelocity;
            return;
        }

        UpdateTargetOffsetTimer();
        MoveToTargetArea();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (direction.sqrMagnitude <= 0.01f)
            return;

        knockbackVelocity = direction.normalized * force;
        knockbackTimer = duration;
    }

    private void UpdateTargetOffsetTimer()
    {
        offsetTimer -= Time.fixedDeltaTime;

        if (offsetTimer <= 0f)
        {
            PickNewTargetOffset();
            offsetTimer = offsetChangeInterval;
        }
    }

    private void PickNewTargetOffset()
    {
        Vector2 direction = Random.insideUnitCircle;

        if (direction.sqrMagnitude <= 0.01f)
        {
            direction = Vector2.right;
        }

        direction.Normalize();

        float radius = Random.Range(targetOffsetMinRadius, targetOffsetMaxRadius);
        personalTargetOffset = direction * radius;
    }

    private void MoveToTargetArea()
    {
        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 currentPosition = rb.position;

        Vector2 desiredTargetPosition = (Vector2)target.position + personalTargetOffset;
        Vector2 toTarget = desiredTargetPosition - currentPosition;

        float distanceToRealPlayer = Vector2.Distance(currentPosition, target.position);
        float distanceToTargetArea = toTarget.magnitude;

        Vector2 chaseDirection = distanceToTargetArea > 0.01f
            ? toTarget.normalized
            : Vector2.zero;

        Vector2 separationDirection = GetSeparationDirection();

        float chaseWeight = distanceToRealPlayer <= nearPlayerDistance
            ? nearPlayerChaseWeight
            : 1f;

        Vector2 finalDirection =
            chaseDirection * chaseWeight +
            separationDirection * separationWeight;

        if (distanceToTargetArea <= stopDistance && separationDirection.sqrMagnitude <= 0.01f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (finalDirection.sqrMagnitude <= 0.01f)
        {
            finalDirection = chaseDirection;
        }

        rb.linearVelocity = finalDirection.normalized * moveSpeed;
    }

    private Vector2 GetSeparationDirection()
    {
        Vector2 separation = Vector2.zero;

        for (int i = 0; i < ActiveEnemies.Count; i++)
        {
            EnemyMovement otherEnemy = ActiveEnemies[i];

            if (otherEnemy == null || otherEnemy == this)
                continue;

            Vector2 away = (Vector2)(transform.position - otherEnemy.transform.position);
            float distanceSqr = away.sqrMagnitude;

            if (distanceSqr <= 0.0001f)
            {
                away = Random.insideUnitCircle.normalized;
                distanceSqr = 0.0001f;
            }

            float radiusSqr = separationRadius * separationRadius;

            if (distanceSqr > radiusSqr)
                continue;

            float distance = Mathf.Sqrt(distanceSqr);
            float strength = 1f - Mathf.Clamp01(distance / separationRadius);

            separation += away.normalized * strength;
        }

        if (separation.magnitude > maxSeparationForce)
        {
            separation = separation.normalized * maxSeparationForce;
        }

        return separation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetOffsetMaxRadius);
    }
}
```

### FILE: Assets/_Project/Scripts/Items/ExpGem.cs

```csharp
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [Header("EXP")]
    [SerializeField] private int expAmount = 5;

    private bool isCollected;

    public void SetExpAmount(int amount)
    {
        expAmount = Mathf.Max(1, amount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerExp playerExp = other.GetComponent<PlayerExp>();

        if (playerExp == null)
            return;

        TryCollect(playerExp);
    }

    public bool TryCollect(PlayerExp playerExp)
    {
        if (isCollected)
            return false;

        if (playerExp == null)
            return false;

        isCollected = true;
        playerExp.AddExp(expAmount);
        Destroy(gameObject);

        return true;
    }
}
```

### FILE: Assets/_Project/Scripts/Items/SmallHealPickup.cs

```csharp
using UnityEngine;

public class SmallHealPickup : MonoBehaviour
{
    [Header("Heal")]
    [SerializeField] private int healAmount = 10;

    private bool isCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth == null)
            return;

        TryCollect(playerHealth);
    }

    public bool TryCollect(PlayerHealth playerHealth)
    {
        if (isCollected)
            return false;

        if (playerHealth == null)
            return false;

        isCollected = true;

        playerHealth.Heal(healAmount);

        Destroy(gameObject);

        return true;
    }
}
```

### FILE: Assets/_Project/Scripts/Player/PlayerController.cs

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Limit")]
    [SerializeField] private float maximumMoveSpeed = 9f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 facingDirection = Vector2.right;

    private Vector2 knockbackVelocity;
    private float knockbackTimer;

    public Vector2 FacingDirection => facingDirection;
    public float MoveSpeed => moveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ReadMoveInput();
        UpdateFacingDirection();

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ReadMoveInput()
    {
        moveInput = Vector2.zero;

        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            moveInput.y += 1f;

        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            moveInput.y -= 1f;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            moveInput.x -= 1f;

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            moveInput.x += 1f;

        moveInput = moveInput.normalized;
    }

    private void UpdateFacingDirection()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            facingDirection = moveInput;
        }
    }

    private void Move()
    {
        if (knockbackTimer > 0f)
        {
            rb.linearVelocity = knockbackVelocity;
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void ImproveMoveSpeed(float bonusRate)
    {
        if (bonusRate <= 0f)
            return;

        moveSpeed *= 1f + bonusRate;
        moveSpeed = Mathf.Min(moveSpeed, maximumMoveSpeed);
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (direction.sqrMagnitude <= 0.01f)
            return;

        knockbackVelocity = direction.normalized * force;
        knockbackTimer = duration;
    }
}
```

### FILE: Assets/_Project/Scripts/Player/PlayerExp.cs

```csharp
using UnityEngine;
using System;

public class PlayerExp : MonoBehaviour
{
    public event Action OnExpChanged;

    [Header("Level")]
    [SerializeField] private int level = 1;
    [SerializeField] private int currentExp = 0;
    [SerializeField] private int expToNextLevel = 20;

    [Header("UI")]
    [SerializeField] private LevelUpUI levelUpUI;

    public int Level => level;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => expToNextLevel;

    public void AddExp(int amount)
    {
        if (amount <= 0)
            return;

        currentExp += amount;

        Debug.Log($"EXP +{amount} / {currentExp}/{expToNextLevel}");

        int levelUpCount = 0;

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
            levelUpCount++;
        }

        if (levelUpCount > 0 && levelUpUI != null)
        {
            levelUpUI.Open(levelUpCount);
        }

        NotifyExpChanged();
    }

    private void LevelUp()
    {
        level++;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.35f + 5f);

        Debug.Log($"Level Up! Current Level: {level}");
    }

    private void NotifyExpChanged()
    {
        OnExpChanged?.Invoke();
    }
}
```

### FILE: Assets/_Project/Scripts/Player/PlayerHealth.cs

```csharp
using System.Collections;
using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealth : MonoBehaviour
{
    public event Action OnHealthChanged;
    public event Action OnDied;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Defense")]
    [SerializeField] private int defense = 0;
    [SerializeField] private int maximumDefense = 20;

    [Header("Hit Reaction")]
    [SerializeField] private float invincibleDuration = 0.6f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.15f;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float blinkInterval = 0.08f;

    private int currentHealth;
    private bool isDead;
    private bool isInvincible;

    private PlayerController playerController;
    private PlayerMeleeAutoAttack playerWeapon;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public int Defense => defense;
    public float HealthRate => maxHealth <= 0 ? 0f : (float)currentHealth / maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;

        playerController = GetComponent<PlayerController>();
        playerWeapon = GetComponent<PlayerMeleeAutoAttack>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        NotifyHealthChanged();
    }

    public void TakeDamage(int damage)
    {
        TakeDamage(damage, Vector2.zero);
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (isDead || isInvincible)
            return;

        if (damage <= 0)
            return;

        int finalDamage = Mathf.Max(1, damage - defense);

        currentHealth -= finalDamage;
        currentHealth = Mathf.Max(currentHealth, 0);
        NotifyHealthChanged();

        Debug.Log($"Player took {finalDamage} damage. Raw: {damage}, Defense: {defense}, HP: {currentHealth}/{maxHealth}");

        if (hitDirection.sqrMagnitude > 0.01f && playerController != null)
        {
            playerController.ApplyKnockback(hitDirection, knockbackForce, knockbackDuration);
        }

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibleRoutine());
    }

    public void AddMaxHealth(int amount, bool healSameAmount)
    {
        if (amount <= 0)
            return;

        maxHealth += amount;

        if (healSameAmount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }

        Debug.Log($"Max HP increased. HP: {currentHealth}/{maxHealth}");
        NotifyHealthChanged();
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
            return;

        if (isDead)
            return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log($"Player healed {amount}. HP: {currentHealth}/{maxHealth}");
        NotifyHealthChanged();
    }

    public void AddDefense(int amount)
    {
        if (amount <= 0)
            return;

        defense += amount;
        defense = Mathf.Min(defense, maximumDefense);

        Debug.Log($"Defense increased. Defense: {defense}");
    }

    private IEnumerator InvincibleRoutine()
    {
        isInvincible = true;

        float timer = 0f;

        while (timer < invincibleDuration)
        {
            if (spriteRenderer != null)
                spriteRenderer.color = hitColor;

            yield return new WaitForSeconds(blinkInterval);

            if (spriteRenderer != null)
                spriteRenderer.color = originalColor;

            yield return new WaitForSeconds(blinkInterval);

            timer += blinkInterval * 2f;
        }

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        isInvincible = false;
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (playerController != null)
            playerController.enabled = false;

        if (playerWeapon != null)
            playerWeapon.enabled = false;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.gray;

        Time.timeScale = 0f;
        NotifyHealthChanged();
        OnDied?.Invoke();

        Debug.Log("Game Over. Player died.");
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke();
    }
}
```

### FILE: Assets/_Project/Scripts/Player/PlayerPickupRange.cs

```csharp
using UnityEngine;

[RequireComponent(typeof(PlayerExp))]
public class PlayerPickupRange : MonoBehaviour
{
    [Header("Pickup")]
    [SerializeField] private float pickupRadius = 1.5f;
    [SerializeField] private float maximumPickupRadius = 6f;
    [SerializeField] private float scanInterval = 0.1f;
    [SerializeField] private LayerMask targetLayers = ~0;

    private PlayerExp playerExp;
    private float scanTimer;

    public float PickupRadius => pickupRadius;

    private void Awake()
    {
        playerExp = GetComponent<PlayerExp>();
    }

    private void Update()
    {
        scanTimer -= Time.deltaTime;

        if (scanTimer > 0f)
            return;

        scanTimer = scanInterval;
        ScanForExpGems();
    }

    private void ScanForExpGems()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRadius, targetLayers);

        for (int i = 0; i < hits.Length; i++)
        {
            ExpGem expGem = hits[i].GetComponent<ExpGem>();

            if (expGem == null)
                continue;

            expGem.TryCollect(playerExp);
        }
    }

    public void ImprovePickupRange(float bonusRate)
    {
        if (bonusRate <= 0f)
            return;

        pickupRadius *= 1f + bonusRate;
        pickupRadius = Mathf.Min(pickupRadius, maximumPickupRadius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
```

### FILE: Assets/_Project/Scripts/Projectiles/MagicBoltProjectile.cs

```csharp
using UnityEngine;

public class MagicBoltProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float lifeTime = 3f;

    [Header("Hit")]
    [SerializeField] private LayerMask enemyLayer;

    private int damage;
    private Vector2 moveDirection;
    private bool hasHit;
    private float lifeTimer;
    private PlayerMagicBoltAutoAttack owner;

    public void SetOwner(PlayerMagicBoltAutoAttack projectileOwner)
    {
        owner = projectileOwner;
    }

    public void Initialize(Vector2 direction, int newDamage, LayerMask newEnemyLayer)
    {
        moveDirection = direction.normalized;
        damage = Mathf.Max(1, newDamage);
        enemyLayer = newEnemyLayer;
        hasHit = false;
        lifeTimer = lifeTime;
    }

    private void Update()
    {
        if (hasHit)
            return;

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            Release();
            return;
        }

        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        bool isEnemyLayer = ((1 << other.gameObject.layer) & enemyLayer.value) != 0;

        if (!isEnemyLayer)
            return;

        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (enemyHealth == null)
            return;

        hasHit = true;

        enemyHealth.TakeDamage(damage, transform.position);

        Release();
    }

    private void OnDisable()
    {
        hasHit = true;
    }

    private void Release()
    {
        hasHit = true;

        if (owner != null)
        {
            owner.ReturnProjectileToPool(this);
            return;
        }

        Destroy(gameObject);
    }
}
```

### FILE: Assets/_Project/Scripts/Relics/PlayerRelicEffects.cs

```csharp
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMeleeAutoAttack))]
public class PlayerRelicEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMeleeAutoAttack playerWeapon;

    [Header("Berserker Fang")]
    [SerializeField] private bool hasBerserkerFang;
    [SerializeField] private float berserkerAttackSpeedBonus = 0.25f;
    [SerializeField] private float berserkerHpThreshold = 0.4f;

    [Header("Blood Sigil")]
    [SerializeField] private bool hasBloodSigil;
    [SerializeField, Range(0f, 1f)] private float bloodSigilHealChance = 0.05f;
    [SerializeField] private int bloodSigilHealAmount = 1;

    private void Awake()
    {
        if (playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

        if (playerWeapon == null)
        {
            playerWeapon = GetComponent<PlayerMeleeAutoAttack>();
        }
    }

    private void Update()
    {
        UpdateBerserkerFang();
    }

    public void ActivateBerserkerFang(float attackSpeedBonus, float hpThreshold)
    {
        hasBerserkerFang = true;
        berserkerAttackSpeedBonus = Mathf.Max(0f, attackSpeedBonus);
        berserkerHpThreshold = Mathf.Clamp01(hpThreshold);

        Debug.Log($"Berserker Fang activated. Bonus: {berserkerAttackSpeedBonus}, HP Threshold: {berserkerHpThreshold}");
    }

    public void ActivateBloodSigil(float healChance, int healAmount)
    {
        hasBloodSigil = true;
        bloodSigilHealChance = Mathf.Clamp01(healChance);
        bloodSigilHealAmount = Mathf.Max(1, healAmount);

        Debug.Log($"Blood Sigil activated. Chance: {bloodSigilHealChance}, Heal: {bloodSigilHealAmount}");
    }

    public void OnEnemyKilled()
    {
        if (!hasBloodSigil)
            return;

        if (playerHealth == null)
            return;

        if (Random.value > bloodSigilHealChance)
            return;

        playerHealth.Heal(bloodSigilHealAmount);

        Debug.Log("Blood Sigil healed player.");
    }

    private void UpdateBerserkerFang()
    {
        if (playerWeapon == null)
            return;

        if (!hasBerserkerFang || playerHealth == null)
        {
            playerWeapon.SetRelicAttackSpeedBonus(0f);
            return;
        }

        if (playerHealth.HealthRate <= berserkerHpThreshold)
        {
            playerWeapon.SetRelicAttackSpeedBonus(berserkerAttackSpeedBonus);
        }
        else
        {
            playerWeapon.SetRelicAttackSpeedBonus(0f);
        }
    }
}
```

### FILE: Assets/_Project/Scripts/Relics/RelicData.cs

```csharp
using UnityEngine;

public enum RelicType
{
    BerserkerFang,
    IronCharm,
    BloodSigil,
    HunterEye,
    GraveMagnet
}

[CreateAssetMenu(fileName = "Relic_", menuName = "_Project/Relics/Relic Data")]
public class RelicData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string relicName;
    [SerializeField, TextArea(2, 5)] private string description;
    [SerializeField] private Sprite icon;

    [Header("Effect")]
    [SerializeField] private RelicType relicType;
    [SerializeField] private int intValue;
    [SerializeField] private float floatValue;
    [SerializeField] private float secondaryFloatValue;

    [Header("Stack")]
    [SerializeField] private bool canStack;
    [SerializeField] private int maxStack = 1;

    public string RelicName => relicName;
    public string Description => description;
    public Sprite Icon => icon;

    public RelicType RelicType => relicType;
    public int IntValue => intValue;
    public float FloatValue => floatValue;
    public float SecondaryFloatValue => secondaryFloatValue;

    public bool CanStack => canStack;
    public int MaxStack => maxStack;
}
```

### FILE: Assets/_Project/Scripts/Spawning/EnemySpawner.cs

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    public class EnemySpawnEntry
    {
        public bool enabled = true;
        public string displayName;
        public GameObject enemyPrefab;

        [Min(0f)]
        public float spawnWeight = 1f;
    }

    [Header("References")]
    [SerializeField] private Transform playerTarget;
    [SerializeField] private WaveManager waveManager;

    [Header("Enemy Spawn Entries")]
    [SerializeField] private List<EnemySpawnEntry> enemySpawnEntries = new List<EnemySpawnEntry>();

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float spawnRadius = 8f;
    [SerializeField] private int maxEnemies = 15;
    [SerializeField] private bool useEnemyPooling = true;
    [SerializeField] private int prewarmPerEnemyType = 6;

    private readonly List<GameObject> spawnedEnemies = new List<GameObject>();
    private readonly Dictionary<GameObject, Queue<GameObject>> enemyPoolByPrefab = new Dictionary<GameObject, Queue<GameObject>>();
    private readonly Dictionary<GameObject, GameObject> activeEnemyPrefabMap = new Dictionary<GameObject, GameObject>();

    private float spawnTimer;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        if (playerTarget != null)
        {
            playerHealth = playerTarget.GetComponent<PlayerHealth>();
        }

        if (useEnemyPooling)
        {
            BuildEnemyPool();
        }
    }

    private void Update()
    {
        if (playerTarget == null || waveManager == null)
            return;

        if (!HasValidEnemyPrefab())
            return;

        if (playerHealth != null && playerHealth.IsDead)
            return;

        RemoveDeadEnemies();

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            TrySpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    private bool HasValidEnemyPrefab()
    {
        for (int i = 0; i < enemySpawnEntries.Count; i++)
        {
            EnemySpawnEntry entry = enemySpawnEntries[i];

            if (entry == null)
                continue;

            if (!entry.enabled)
                continue;

            if (entry.enemyPrefab == null)
                continue;

            if (entry.spawnWeight <= 0f)
                continue;

            return true;
        }

        return false;
    }

    private void TrySpawnEnemy()
    {
        if (waveManager.ShouldSpawnFirstMidBoss())
        {
            SpawnEnemy(true);
            waveManager.MarkFirstMidBossSpawned();
            return;
        }

        if (spawnedEnemies.Count >= maxEnemies)
            return;

        SpawnEnemy(false);
    }

    private void SpawnEnemy(bool isMidBoss)
    {
        GameObject selectedPrefab = GetRandomEnemyPrefab();

        if (selectedPrefab == null)
            return;

        Vector2 spawnPosition = GetSpawnPositionAroundPlayer();

        GameObject enemy = GetOrCreateEnemy(selectedPrefab, spawnPosition);

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();

        if (enemyMovement != null)
        {
            enemyMovement.SetTarget(playerTarget);
        }

        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.Initialize(
                waveManager.GetEnemyHealth(isMidBoss),
                waveManager.GetExpReward(isMidBoss),
                isMidBoss,
                this
            );
        }

        EnemyContactDamage contactDamage = enemy.GetComponent<EnemyContactDamage>();

        if (contactDamage != null)
        {
            contactDamage.Initialize(waveManager.GetEnemyDamage(isMidBoss));
        }

        spawnedEnemies.Add(enemy);

        if (isMidBoss)
        {
            Debug.Log($"Mid Boss spawned at {waveManager.FirstMidBossSpawnTime} seconds.");
        }
    }

    public void DespawnEnemy(GameObject enemy)
    {
        if (enemy == null)
            return;

        spawnedEnemies.Remove(enemy);

        if (!useEnemyPooling)
        {
            Destroy(enemy);
            return;
        }

        if (activeEnemyPrefabMap.TryGetValue(enemy, out GameObject prefabKey) &&
            prefabKey != null &&
            enemyPoolByPrefab.TryGetValue(prefabKey, out Queue<GameObject> pool))
        {
            enemy.SetActive(false);
            pool.Enqueue(enemy);
            activeEnemyPrefabMap.Remove(enemy);
            return;
        }

        Destroy(enemy);
    }

    private GameObject GetRandomEnemyPrefab()
    {
        float totalWeight = GetTotalSpawnWeight();

        if (totalWeight <= 0f)
            return null;

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        for (int i = 0; i < enemySpawnEntries.Count; i++)
        {
            EnemySpawnEntry entry = enemySpawnEntries[i];

            if (!IsValidEntry(entry))
                continue;

            currentWeight += entry.spawnWeight;

            if (randomValue <= currentWeight)
                return entry.enemyPrefab;
        }

        return null;
    }

    private float GetTotalSpawnWeight()
    {
        float totalWeight = 0f;

        for (int i = 0; i < enemySpawnEntries.Count; i++)
        {
            EnemySpawnEntry entry = enemySpawnEntries[i];

            if (!IsValidEntry(entry))
                continue;

            totalWeight += entry.spawnWeight;
        }

        return totalWeight;
    }

    private bool IsValidEntry(EnemySpawnEntry entry)
    {
        if (entry == null)
            return false;

        if (!entry.enabled)
            return false;

        if (entry.enemyPrefab == null)
            return false;

        if (entry.spawnWeight <= 0f)
            return false;

        return true;
    }

    private Vector2 GetSpawnPositionAroundPlayer()
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;

        if (randomDirection.sqrMagnitude <= 0.01f)
        {
            randomDirection = Vector2.right;
        }

        Vector2 playerPosition = playerTarget.position;

        return playerPosition + randomDirection * spawnRadius;
    }

    private void RemoveDeadEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null || !spawnedEnemies[i].activeInHierarchy)
            {
                spawnedEnemies.RemoveAt(i);
            }
        }
    }

    private void BuildEnemyPool()
    {
        enemyPoolByPrefab.Clear();
        activeEnemyPrefabMap.Clear();

        for (int i = 0; i < enemySpawnEntries.Count; i++)
        {
            EnemySpawnEntry entry = enemySpawnEntries[i];

            if (!IsValidEntry(entry))
                continue;

            if (enemyPoolByPrefab.ContainsKey(entry.enemyPrefab))
                continue;

            Queue<GameObject> queue = new Queue<GameObject>();
            enemyPoolByPrefab.Add(entry.enemyPrefab, queue);

            for (int j = 0; j < prewarmPerEnemyType; j++)
            {
                GameObject pooledEnemy = Instantiate(entry.enemyPrefab, Vector3.zero, Quaternion.identity);
                pooledEnemy.SetActive(false);
                queue.Enqueue(pooledEnemy);
            }
        }
    }

    private GameObject GetOrCreateEnemy(GameObject prefab, Vector2 spawnPosition)
    {
        if (!useEnemyPooling)
        {
            return Instantiate(prefab, spawnPosition, Quaternion.identity);
        }

        if (!enemyPoolByPrefab.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            enemyPoolByPrefab.Add(prefab, pool);
        }

        GameObject enemy = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab, spawnPosition, Quaternion.identity);
        enemy.transform.position = spawnPosition;
        enemy.transform.rotation = Quaternion.identity;
        enemy.SetActive(true);
        activeEnemyPrefabMap[enemy] = prefab;
        return enemy;
    }
}
```

### FILE: Assets/_Project/Scripts/Spawning/WaveManager.cs

```csharp
using UnityEngine;
using System;

public class WaveManager : MonoBehaviour
{
    public event Action OnWaveChanged;

    [Header("References")]
    [SerializeField] private GameTimer gameTimer;

    [Header("Wave")]
    [SerializeField] private int currentWave = 1;
    [SerializeField] private float waveDuration = 30f;

    [Header("Difficulty")]
    [SerializeField] private int baseEnemyHealth = 30;
    [SerializeField] private int healthPerWave = 5;
    [SerializeField] private int baseEnemyDamage = 8;
    [SerializeField] private int damagePerWave = 1;
    [SerializeField] private int baseExpReward = 5;

    [Header("Timed Mid Boss")]
    [SerializeField] private float firstMidBossSpawnTime = 300f;
    [SerializeField] private float midBossHealthMultiplier = 3f;
    [SerializeField] private float midBossDamageMultiplier = 1.5f;
    [SerializeField] private float midBossExpMultiplier = 4f;

    private float waveTimer;
    private bool firstMidBossSpawned;

    public int CurrentWave => currentWave;
    public float WaveTimer => waveTimer;
    public float WaveDuration => waveDuration;
    public float FirstMidBossSpawnTime => firstMidBossSpawnTime;
    public bool FirstMidBossSpawned => firstMidBossSpawned;

    private void Awake()
    {
        if (gameTimer == null)
        {
            gameTimer = FindFirstObjectByType<GameTimer>();
        }

        waveTimer = waveDuration;
        OnWaveChanged?.Invoke();
    }

    private void Update()
    {
        waveTimer -= Time.deltaTime;

        if (waveTimer <= 0f)
        {
            AdvanceWave();
        }
    }

    private void AdvanceWave()
    {
        currentWave++;
        waveTimer = waveDuration;
        OnWaveChanged?.Invoke();

        Debug.Log($"Wave {currentWave} started.");
    }

    public bool ShouldSpawnFirstMidBoss()
    {
        if (firstMidBossSpawned)
            return false;

        if (gameTimer == null)
            return false;

        return gameTimer.GameplayTime >= firstMidBossSpawnTime;
    }

    public void MarkFirstMidBossSpawned()
    {
        firstMidBossSpawned = true;
        OnWaveChanged?.Invoke();
    }

    // 기존 HUDCanvasUI / HUDDebugUI 호환용
    public bool IsMidBossWave()
    {
        return ShouldSpawnFirstMidBoss();
    }

    public int GetEnemyHealth(bool isMidBoss)
    {
        int health = baseEnemyHealth + (currentWave - 1) * healthPerWave;

        if (isMidBoss)
            health = Mathf.RoundToInt(health * midBossHealthMultiplier);

        return health;
    }

    public int GetEnemyDamage(bool isMidBoss)
    {
        int damage = baseEnemyDamage + (currentWave - 1) * damagePerWave;

        if (isMidBoss)
            damage = Mathf.RoundToInt(damage * midBossDamageMultiplier);

        return damage;
    }

    public int GetExpReward(bool isMidBoss)
    {
        int exp = baseExpReward + currentWave;

        if (isMidBoss)
            exp = Mathf.RoundToInt(exp * midBossExpMultiplier);

        return exp;
    }
}
```

### FILE: Assets/_Project/Scripts/UI/HUDCanvasUI.cs

```csharp
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDCanvasUI : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerExp playerExp;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private RelicSelectUI relicSelectUI;

    [Header("HP UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;

    [Header("EXP UI")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private TMP_Text expText;

    [Header("Wave UI")]
    [SerializeField] private TMP_Text waveText;

    [Header("Relic UI")]
    [SerializeField] private TMP_Text relicText;
    [SerializeField] private string relicTitle = "RELICS";
    [SerializeField] private string emptyRelicText = "RELICS\n- None";
    [SerializeField] private float waveUiRefreshInterval = 0.2f;

    private readonly StringBuilder relicTextBuilder = new StringBuilder();
    private float waveUiRefreshTimer;

    private void Awake()
    {
        if (relicSelectUI == null)
        {
            relicSelectUI = FindFirstObjectByType<RelicSelectUI>();
        }

        if (waveUiRefreshInterval <= 0f)
        {
            waveUiRefreshInterval = 0.2f;
        }
    }

    private void OnEnable()
    {
        BindEvents();
        RefreshAllImmediate();
    }

    private void OnDisable()
    {
        UnbindEvents();
    }

    private void Update()
    {
        // Wave 카운트다운은 초 단위 변화가 있어 저주기로만 갱신합니다.
        waveUiRefreshTimer -= Time.unscaledDeltaTime;

        if (waveUiRefreshTimer <= 0f)
        {
            waveUiRefreshTimer = waveUiRefreshInterval;
            UpdateWaveUI();
        }
    }

    private void BindEvents()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHpUI;
        }

        if (playerExp != null)
        {
            playerExp.OnExpChanged += UpdateExpUI;
        }

        if (relicSelectUI != null)
        {
            relicSelectUI.OnOwnedRelicsChanged += UpdateRelicUI;
        }

        if (waveManager != null)
        {
            waveManager.OnWaveChanged += UpdateWaveUI;
        }
    }

    private void UnbindEvents()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHpUI;
        }

        if (playerExp != null)
        {
            playerExp.OnExpChanged -= UpdateExpUI;
        }

        if (relicSelectUI != null)
        {
            relicSelectUI.OnOwnedRelicsChanged -= UpdateRelicUI;
        }

        if (waveManager != null)
        {
            waveManager.OnWaveChanged -= UpdateWaveUI;
        }
    }

    private void RefreshAllImmediate()
    {
        UpdateHpUI();
        UpdateExpUI();
        UpdateWaveUI();
        UpdateRelicUI();
        waveUiRefreshTimer = waveUiRefreshInterval;
    }

    private void UpdateHpUI()
    {
        if (playerHealth == null || hpSlider == null || hpText == null)
            return;

        float hpRatio = 0f;

        if (playerHealth.MaxHealth > 0)
        {
            hpRatio = (float)playerHealth.CurrentHealth / playerHealth.MaxHealth;
        }

        hpSlider.value = Mathf.Clamp01(hpRatio);
        hpText.text = $"HP {playerHealth.CurrentHealth} / {playerHealth.MaxHealth}";
    }

    private void UpdateExpUI()
    {
        if (playerExp == null || expSlider == null || expText == null)
            return;

        float expRatio = 0f;

        if (playerExp.ExpToNextLevel > 0)
        {
            expRatio = (float)playerExp.CurrentExp / playerExp.ExpToNextLevel;
        }

        expSlider.value = Mathf.Clamp01(expRatio);
        expText.text = $"LV {playerExp.Level}  EXP {playerExp.CurrentExp} / {playerExp.ExpToNextLevel}";
    }

    private void UpdateWaveUI()
    {
        if (waveManager == null || waveText == null)
            return;

        if (waveManager.IsMidBossWave())
        {
            waveText.text = $"WAVE {waveManager.CurrentWave}  MID BOSS";
        }
        else
        {
            waveText.text = $"WAVE {waveManager.CurrentWave}  NEXT {Mathf.CeilToInt(waveManager.WaveTimer)}s";
        }
    }

    private void UpdateRelicUI()
    {
        if (relicText == null)
            return;

        if (relicSelectUI == null)
        {
            relicSelectUI = FindFirstObjectByType<RelicSelectUI>();
        }

        if (relicSelectUI == null)
        {
            relicText.text = emptyRelicText;
            return;
        }

        IReadOnlyList<RelicData> ownedRelics = relicSelectUI.OwnedRelics;

        if (ownedRelics == null || ownedRelics.Count <= 0)
        {
            relicText.text = emptyRelicText;
            return;
        }

        relicTextBuilder.Clear();
        relicTextBuilder.AppendLine(relicTitle);

        for (int i = 0; i < ownedRelics.Count; i++)
        {
            RelicData relic = ownedRelics[i];

            if (relic == null)
                continue;

            relicTextBuilder.Append("- ");
            relicTextBuilder.AppendLine(relic.RelicName);
        }

        relicText.text = relicTextBuilder.ToString();
    }
}
```

### FILE: Assets/_Project/Scripts/UI/LevelUpUI.cs

```csharp
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private PlayerMeleeAutoAttack playerWeapon;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerPickupRange playerPickupRange;
    [SerializeField] private PlayerMagicBoltAutoAttack playerMagicBolt;

    [Header("Upgrade Data")]
    [SerializeField] private List<UpgradeData> availableUpgrades = new List<UpgradeData>();

    [Header("UI Root")]
    [SerializeField] private GameObject levelUpPanel;

    [Header("Texts")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text pendingText;

    [Header("Buttons")]
    [SerializeField] private Button damageButton;
    [SerializeField] private Button attackSpeedButton;
    [SerializeField] private Button attackRangeButton;

    [Header("Button Texts")]
    [SerializeField] private TMP_Text damageButtonText;
    [SerializeField] private TMP_Text attackSpeedButtonText;
    [SerializeField] private TMP_Text attackRangeButtonText;

    private readonly List<UpgradeData> currentChoices = new List<UpgradeData>();

    private int pendingLevelUps;
    private bool isOpen;

    private void Awake()
    {
        AutoBindIfNeeded();
        ValidateRequiredReferences();
        RegisterButtonEvents();
        ClosePanelOnly();
    }

    private void ValidateRequiredReferences()
    {
        if (levelUpPanel == null)
        {
            Debug.LogWarning("[LevelUpUI] levelUpPanel이 비어 있습니다. Hierarchy의 LevelUpPanel 연결을 확인하세요.", this);
        }

        if (damageButton == null || attackSpeedButton == null || attackRangeButton == null)
        {
            Debug.LogWarning("[LevelUpUI] 강화 선택 버튼 참조가 일부 비어 있습니다. Damage/AttackSpeed/AttackRange 버튼 연결을 확인하세요.", this);
        }

        if (damageButtonText == null || attackSpeedButtonText == null || attackRangeButtonText == null)
        {
            Debug.LogWarning("[LevelUpUI] 버튼 TMP_Text 참조가 일부 비어 있습니다. damage/attackSpeed/attackRangeButtonText 필드 연결을 확인하세요.", this);
        }

        if (playerHealth == null)
        {
            Debug.LogWarning("[LevelUpUI] playerHealth가 비어 있습니다. Player 오브젝트의 PlayerHealth를 연결하세요.", this);
        }

        if (pauseManager == null)
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        if (pauseManager == null)
        {
            Debug.LogWarning("[LevelUpUI] pauseManager가 비어 있습니다. PauseManager 연결을 권장합니다. (없으면 Time.timeScale 폴백 사용)", this);
        }
    }

    private void AutoBindIfNeeded()
    {
        if (levelUpPanel == null)
        {
            levelUpPanel = transform.Find("LevelUpPanel")?.gameObject;
        }

        if (titleText == null)
        {
            titleText = FindTMP("TitleText");
        }

        if (pendingText == null)
        {
            pendingText = FindTMP("PendingText");
        }

        if (damageButton == null)
        {
            damageButton = FindButton("DamageButton");
        }

        if (attackSpeedButton == null)
        {
            attackSpeedButton = FindButton("AttackSpeedButton");
        }

        if (attackRangeButton == null)
        {
            attackRangeButton = FindButton("AttackRangeButton");
        }

        if (damageButtonText == null && damageButton != null)
        {
            damageButtonText = damageButton.GetComponentInChildren<TMP_Text>(true);
        }

        if (attackSpeedButtonText == null && attackSpeedButton != null)
        {
            attackSpeedButtonText = attackSpeedButton.GetComponentInChildren<TMP_Text>(true);
        }

        if (attackRangeButtonText == null && attackRangeButton != null)
        {
            attackRangeButtonText = attackRangeButton.GetComponentInChildren<TMP_Text>(true);
        }

        if (playerController == null && playerHealth != null)
        {
            playerController = playerHealth.GetComponent<PlayerController>();
        }

        if (playerController == null && playerWeapon != null)
        {
            playerController = playerWeapon.GetComponent<PlayerController>();
        }

        if (playerPickupRange == null && playerHealth != null)
        {
            playerPickupRange = playerHealth.GetComponent<PlayerPickupRange>();
        }

        if (playerPickupRange == null && playerController != null)
        {
            playerPickupRange = playerController.GetComponent<PlayerPickupRange>();
        }

        if (playerMagicBolt == null && playerHealth != null)
        {
            playerMagicBolt = playerHealth.GetComponent<PlayerMagicBoltAutoAttack>();
        }

        if (playerMagicBolt == null && playerController != null)
        {
            playerMagicBolt = playerController.GetComponent<PlayerMagicBoltAutoAttack>();
        }
    }

    private TMP_Text FindTMP(string objectName)
    {
        Transform target = transform.Find("LevelUpPanel/" + objectName);

        if (target == null)
        {
            return null;
        }

        return target.GetComponent<TMP_Text>();
    }

    private Button FindButton(string objectName)
    {
        Transform target = transform.Find("LevelUpPanel/" + objectName);

        if (target == null)
        {
            return null;
        }

        return target.GetComponent<Button>();
    }

    private void RegisterButtonEvents()
    {
        if (damageButton != null)
        {
            damageButton.onClick.RemoveAllListeners();
            damageButton.onClick.AddListener(() => ChooseUpgrade(0));
        }

        if (attackSpeedButton != null)
        {
            attackSpeedButton.onClick.RemoveAllListeners();
            attackSpeedButton.onClick.AddListener(() => ChooseUpgrade(1));
        }

        if (attackRangeButton != null)
        {
            attackRangeButton.onClick.RemoveAllListeners();
            attackRangeButton.onClick.AddListener(() => ChooseUpgrade(2));
        }
    }

    public void Open(int levelUpCount)
    {
        if (levelUpPanel == null)
        {
            Debug.LogWarning("[LevelUpUI] levelUpPanel이 없어 레벨업 UI를 열 수 없습니다.", this);
            return;
        }

        pendingLevelUps += levelUpCount;

        if (pendingLevelUps <= 0)
        {
            return;
        }

        isOpen = true;
        RequestPause();

        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);
        }

        PickChoices();
        UpdateTexts();
    }

    private void PickChoices()
    {
        currentChoices.Clear();

        List<UpgradeData> pool = new List<UpgradeData>();

        for (int i = 0; i < availableUpgrades.Count; i++)
        {
            UpgradeData upgrade = availableUpgrades[i];

            if (upgrade == null)
                continue;

            if (!CanAppearInChoices(upgrade))
                continue;

            pool.Add(upgrade);
        }

        int choiceCount = Mathf.Min(3, pool.Count);

        for (int i = 0; i < choiceCount; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            currentChoices.Add(pool[randomIndex]);
            pool.RemoveAt(randomIndex);
        }
    }

    private bool CanAppearInChoices(UpgradeData upgrade)
    {
        if (upgrade == null)
            return false;

        if (upgrade.UpgradeType != UpgradeType.WeaponUnlock)
            return true;

        if (upgrade.WeaponId == "magic_bolt")
        {
            if (playerMagicBolt == null)
                return true;

            return !playerMagicBolt.IsUnlocked;
        }

        return true;
    }

    private void ChooseUpgrade(int choiceIndex)
    {
        if (!CanChoose())
        {
            return;
        }

        if (choiceIndex < 0 || choiceIndex >= currentChoices.Count)
        {
            return;
        }

        ApplyUpgrade(currentChoices[choiceIndex]);
        CompleteOneChoice();
    }

    private bool CanChoose()
    {
        return isOpen && pendingLevelUps > 0;
    }

    private void ApplyUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null)
        {
            return;
        }

        switch (upgrade.UpgradeType)
        {
            case UpgradeType.Damage:
                if (playerWeapon != null)
                {
                    playerWeapon.AddDamage(upgrade.IntValue);
                }

                if (playerMagicBolt != null)
                {
                    playerMagicBolt.AddDamage(upgrade.IntValue);
                }
                break;

            case UpgradeType.AttackSpeed:
                if (playerWeapon != null)
                {
                    playerWeapon.ImproveAttackSpeed(upgrade.FloatValue);
                }

                if (playerMagicBolt != null)
                {
                    playerMagicBolt.ImproveAttackSpeed(upgrade.FloatValue);
                }
                break;

            case UpgradeType.AttackRange:
                if (playerWeapon != null)
                {
                    playerWeapon.ImproveAttackRange(upgrade.FloatValue);
                }

                if (playerMagicBolt != null)
                {
                    playerMagicBolt.ImproveAttackRange(upgrade.FloatValue);
                }
                break;

            case UpgradeType.MaxHealth:
                if (playerHealth != null)
                {
                    playerHealth.AddMaxHealth(upgrade.IntValue, true);
                }
                break;

            case UpgradeType.MoveSpeed:
                if (playerController != null)
                {
                    playerController.ImproveMoveSpeed(upgrade.FloatValue);
                }
                break;

            case UpgradeType.PickupRange:
                if (playerPickupRange != null)
                {
                    playerPickupRange.ImprovePickupRange(upgrade.FloatValue);
                }
                break;

            case UpgradeType.Defense:
                if (playerHealth != null)
                {
                    playerHealth.AddDefense(upgrade.IntValue);
                }
                break;

            case UpgradeType.CriticalChance:
                if (playerWeapon != null)
                {
                    playerWeapon.AddCriticalChance(upgrade.FloatValue);
                }
                break;

            case UpgradeType.WeaponUnlock:
                UnlockWeapon(upgrade.WeaponId);
                break;
        }
    }

    private void UnlockWeapon(string weaponId)
    {
        if (string.IsNullOrEmpty(weaponId))
        {
            Debug.LogWarning("Weapon unlock failed. WeaponId is empty.");
            return;
        }

        switch (weaponId)
        {
            case "magic_bolt":
                if (playerMagicBolt != null)
                {
                    playerMagicBolt.Unlock();
                }
                else
                {
                    Debug.LogWarning("Magic Bolt unlock failed. PlayerMagicBoltAutoAttack is not found on Player.");
                }
                break;

            default:
                Debug.LogWarning($"Unknown weapon id: {weaponId}");
                break;
        }
    }

    private void CompleteOneChoice()
    {
        pendingLevelUps--;

        if (pendingLevelUps > 0)
        {
            PickChoices();
            UpdateTexts();
            return;
        }

        isOpen = false;
        ReleasePause();
        ClosePanelOnly();
    }

    private void OnDisable()
    {
        if (isOpen)
        {
            ReleasePause();
            isOpen = false;
        }
    }

    private void RequestPause()
    {
        if (pauseManager != null)
        {
            pauseManager.RequestPause(this);
            return;
        }

        Time.timeScale = 0f;
    }

    private void ReleasePause()
    {
        if (pauseManager != null)
        {
            pauseManager.ReleasePause(this);
            return;
        }

        Time.timeScale = 1f;
    }

    private void ClosePanelOnly()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }
    }

    private void UpdateTexts()
    {
        if (titleText != null)
        {
            titleText.text = "LEVEL UP";
        }

        if (pendingText != null)
        {
            pendingText.text = "Choices Left: " + pendingLevelUps;
        }

        SetButtonText(damageButtonText, 0);
        SetButtonText(attackSpeedButtonText, 1);
        SetButtonText(attackRangeButtonText, 2);
    }

    private void SetButtonText(TMP_Text buttonText, int choiceIndex)
    {
        if (buttonText == null)
        {
            return;
        }

        if (choiceIndex < 0 || choiceIndex >= currentChoices.Count)
        {
            buttonText.text = "-";
            return;
        }

        UpgradeData upgrade = currentChoices[choiceIndex];

        buttonText.text = upgrade.UpgradeName + "\n" + upgrade.Description;
    }
}
```

### FILE: Assets/_Project/Scripts/UI/RelicSelectUI.cs

```csharp
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicSelectUI : MonoBehaviour
{
    public event Action OnOwnedRelicsChanged;

    [Header("Target")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private PlayerMeleeAutoAttack playerWeapon;
    [SerializeField] private PlayerPickupRange playerPickupRange;
    [SerializeField] private PlayerRelicEffects playerRelicEffects;

    [Header("Relic Data")]
    [SerializeField] private List<RelicData> availableRelics = new List<RelicData>();
    [SerializeField] private bool excludeOwnedRelics = true;

    [Header("UI Root")]
    [SerializeField] private GameObject relicSelectPanel;

    [Header("Texts")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text infoText;

    [Header("Buttons")]
    [SerializeField] private Button relicButton01;
    [SerializeField] private Button relicButton02;
    [SerializeField] private Button relicButton03;

    [Header("Button Texts")]
    [SerializeField] private TMP_Text relicButtonText01;
    [SerializeField] private TMP_Text relicButtonText02;
    [SerializeField] private TMP_Text relicButtonText03;

    private readonly List<RelicData> currentChoices = new List<RelicData>();
    private readonly List<RelicData> ownedRelics = new List<RelicData>();

    private bool isOpen;

    public bool IsOpen => isOpen;
    public IReadOnlyList<RelicData> OwnedRelics => ownedRelics;

    private void Awake()
    {
        AutoBindIfNeeded();
        ValidateRequiredReferences();
        RegisterButtonEvents();
        ClosePanelOnly();
    }

    private void ValidateRequiredReferences()
    {
        if (relicSelectPanel == null)
        {
            Debug.LogWarning("[RelicSelectUI] relicSelectPanel이 비어 있습니다. Hierarchy의 RelicSelectPanel 연결을 확인하세요.", this);
        }

        if (relicButton01 == null || relicButton02 == null || relicButton03 == null)
        {
            Debug.LogWarning("[RelicSelectUI] 유물 선택 버튼 참조가 일부 비어 있습니다. RelicButton_01~03 연결을 확인하세요.", this);
        }

        if (relicButtonText01 == null || relicButtonText02 == null || relicButtonText03 == null)
        {
            Debug.LogWarning("[RelicSelectUI] 버튼 TMP_Text 참조가 일부 비어 있습니다. RelicButtonText01~03 필드 연결을 확인하세요.", this);
        }

        if (playerHealth == null)
        {
            Debug.LogWarning("[RelicSelectUI] playerHealth가 비어 있습니다. Player 오브젝트의 PlayerHealth를 연결하세요.", this);
        }

        if (pauseManager == null)
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        if (pauseManager == null)
        {
            Debug.LogWarning("[RelicSelectUI] pauseManager가 비어 있습니다. PauseManager 연결을 권장합니다. (없으면 Time.timeScale 폴백 사용)", this);
        }
    }

    private void AutoBindIfNeeded()
    {
        if (relicSelectPanel == null)
        {
            relicSelectPanel = transform.Find("RelicSelectPanel")?.gameObject;
        }

        if (titleText == null)
        {
            titleText = FindTMP("TitleText");
        }

        if (infoText == null)
        {
            infoText = FindTMP("InfoText");
        }

        if (relicButton01 == null)
        {
            relicButton01 = FindButton("RelicButton_01");
        }

        if (relicButton02 == null)
        {
            relicButton02 = FindButton("RelicButton_02");
        }

        if (relicButton03 == null)
        {
            relicButton03 = FindButton("RelicButton_03");
        }

        if (relicButtonText01 == null && relicButton01 != null)
        {
            relicButtonText01 = relicButton01.GetComponentInChildren<TMP_Text>(true);
        }

        if (relicButtonText02 == null && relicButton02 != null)
        {
            relicButtonText02 = relicButton02.GetComponentInChildren<TMP_Text>(true);
        }

        if (relicButtonText03 == null && relicButton03 != null)
        {
            relicButtonText03 = relicButton03.GetComponentInChildren<TMP_Text>(true);
        }

        if (playerHealth != null)
        {
            if (playerWeapon == null)
            {
                playerWeapon = playerHealth.GetComponent<PlayerMeleeAutoAttack>();
            }

            if (playerPickupRange == null)
            {
                playerPickupRange = playerHealth.GetComponent<PlayerPickupRange>();
            }

            if (playerRelicEffects == null)
            {
                playerRelicEffects = playerHealth.GetComponent<PlayerRelicEffects>();
            }
        }
    }

    private TMP_Text FindTMP(string objectName)
    {
        Transform target = transform.Find("RelicSelectPanel/" + objectName);

        if (target == null)
            return null;

        return target.GetComponent<TMP_Text>();
    }

    private Button FindButton(string objectName)
    {
        Transform target = transform.Find("RelicSelectPanel/" + objectName);

        if (target == null)
            return null;

        return target.GetComponent<Button>();
    }

    private void RegisterButtonEvents()
    {
        if (relicButton01 != null)
        {
            relicButton01.onClick.RemoveAllListeners();
            relicButton01.onClick.AddListener(() => SelectRelic(0));
        }

        if (relicButton02 != null)
        {
            relicButton02.onClick.RemoveAllListeners();
            relicButton02.onClick.AddListener(() => SelectRelic(1));
        }

        if (relicButton03 != null)
        {
            relicButton03.onClick.RemoveAllListeners();
            relicButton03.onClick.AddListener(() => SelectRelic(2));
        }
    }

    public void Open()
    {
        if (relicSelectPanel == null)
        {
            Debug.LogWarning("[RelicSelectUI] relicSelectPanel이 없어 UI를 열 수 없습니다.", this);
            return;
        }

        PickChoices();

        if (currentChoices.Count <= 0)
        {
            Debug.LogWarning("No relic choices available.");
            return;
        }

        isOpen = true;
        RequestPause();

        if (relicSelectPanel != null)
        {
            relicSelectPanel.SetActive(true);
        }

        UpdateTexts();
    }

    private void PickChoices()
    {
        currentChoices.Clear();

        List<RelicData> pool = new List<RelicData>();

        for (int i = 0; i < availableRelics.Count; i++)
        {
            RelicData relic = availableRelics[i];

            if (relic == null)
                continue;

            if (excludeOwnedRelics && ownedRelics.Contains(relic))
                continue;

            pool.Add(relic);
        }

        int choiceCount = Mathf.Min(3, pool.Count);

        for (int i = 0; i < choiceCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, pool.Count);
            currentChoices.Add(pool[randomIndex]);
            pool.RemoveAt(randomIndex);
        }
    }

    private void SelectRelic(int choiceIndex)
    {
        if (!isOpen)
            return;

        if (choiceIndex < 0 || choiceIndex >= currentChoices.Count)
            return;

        RelicData relic = currentChoices[choiceIndex];

        ApplyRelicEffect(relic);

        if (!ownedRelics.Contains(relic))
        {
            ownedRelics.Add(relic);
            OnOwnedRelicsChanged?.Invoke();
        }

        Debug.Log($"Relic acquired: {relic.RelicName}");

        isOpen = false;
        ReleasePause();

        ClosePanelOnly();
    }

    private void OnDisable()
    {
        if (isOpen)
        {
            ReleasePause();
            isOpen = false;
        }
    }

    private void RequestPause()
    {
        if (pauseManager != null)
        {
            pauseManager.RequestPause(this);
            return;
        }

        Time.timeScale = 0f;
    }

    private void ReleasePause()
    {
        if (pauseManager != null)
        {
            pauseManager.ReleasePause(this);
            return;
        }

        Time.timeScale = 1f;
    }

    private void ApplyRelicEffect(RelicData relic)
    {
        if (relic == null)
            return;

        switch (relic.RelicType)
        {
            case RelicType.IronCharm:
                if (playerHealth != null)
                {
                    playerHealth.AddDefense(relic.IntValue);
                }
                break;

            case RelicType.HunterEye:
                if (playerWeapon != null)
                {
                    playerWeapon.AddCriticalChance(relic.FloatValue);
                }
                break;

            case RelicType.GraveMagnet:
                if (playerPickupRange != null)
                {
                    playerPickupRange.ImprovePickupRange(relic.FloatValue);
                }
                break;

            case RelicType.BerserkerFang:
                if (playerRelicEffects != null)
                {
                    playerRelicEffects.ActivateBerserkerFang(
                        relic.FloatValue,
                        relic.SecondaryFloatValue
                    );
                }
                break;

            case RelicType.BloodSigil:
                if (playerRelicEffects != null)
                {
                    playerRelicEffects.ActivateBloodSigil(
                        relic.FloatValue,
                        relic.IntValue
                    );
                }
                break;
        }
    }

    private void UpdateTexts()
    {
        if (titleText != null)
        {
            titleText.text = "RELIC SELECT";
        }

        if (infoText != null)
        {
            infoText.text = "Choose 1 relic. Its effect lasts during this run.";
        }

        SetButtonText(relicButton01, relicButtonText01, 0);
        SetButtonText(relicButton02, relicButtonText02, 1);
        SetButtonText(relicButton03, relicButtonText03, 2);
    }

    private void SetButtonText(Button button, TMP_Text buttonText, int index)
    {
        bool hasChoice = index >= 0 && index < currentChoices.Count;

        if (button != null)
        {
            button.gameObject.SetActive(hasChoice);
        }

        if (!hasChoice || buttonText == null)
            return;

        RelicData relic = currentChoices[index];

        buttonText.text = relic.RelicName + "\n" + relic.Description;
    }

    private void ClosePanelOnly()
    {
        if (relicSelectPanel != null)
        {
            relicSelectPanel.SetActive(false);
        }
    }
}
```

### FILE: Assets/_Project/Scripts/Weapons/PlayerMagicBoltAutoAttack.cs

```csharp
using UnityEngine;

public class PlayerMagicBoltAutoAttack : MonoBehaviour
{
    [Header("Weapon State")]
    [SerializeField] private bool isUnlocked;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private bool useProjectilePooling = true;
    [SerializeField] private int projectilePoolPrewarmCount = 12;

    [Header("Attack")]
    [SerializeField] private int damage = 8;
    [SerializeField] private float attackInterval = 1.2f;
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Limits")]
    [SerializeField] private float minimumAttackInterval = 0.25f;
    [SerializeField] private float maximumAttackRange = 10f;

    private float attackTimer;
    private readonly System.Collections.Generic.Queue<MagicBoltProjectile> projectilePool = new System.Collections.Generic.Queue<MagicBoltProjectile>();

    public bool IsUnlocked => isUnlocked;
    public int Damage => damage;
    public float AttackInterval => attackInterval;
    public float AttackRange => attackRange;

    private void Awake()
    {
        ValidateRequiredReferences();
        if (useProjectilePooling)
        {
            PrewarmProjectilePool();
        }
        enabled = isUnlocked;
    }

    private void ValidateRequiredReferences()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("[PlayerMagicBoltAutoAttack] projectilePrefab이 비어 있습니다. Inspector에서 매직 볼트 프리팹을 연결하세요.", this);
        }

        if (projectileSpawnPoint == null)
        {
            Debug.LogWarning("[PlayerMagicBoltAutoAttack] projectileSpawnPoint가 비어 있습니다. 기본적으로 Player 위치에서 발사됩니다.", this);
        }
    }

    private void Update()
    {
        if (!isUnlocked)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer > 0f)
            return;

        Transform target = FindNearestEnemy();

        if (target == null)
            return;

        Fire(target);

        attackTimer = attackInterval;
    }

    public void Unlock()
    {
        if (isUnlocked)
            return;

        isUnlocked = true;
        enabled = true;
        attackTimer = 0f;

        Debug.Log("Magic Bolt unlocked.");
    }

    public void AddDamage(int amount)
    {
        if (!isUnlocked)
            return;

        if (amount <= 0)
            return;

        damage += amount;

        Debug.Log($"Magic Bolt damage increased. Damage: {damage}");
    }

    public void ImproveAttackSpeed(float bonusRate)
    {
        if (!isUnlocked)
            return;

        if (bonusRate <= 0f)
            return;

        attackInterval *= 1f - bonusRate;
        attackInterval = Mathf.Max(attackInterval, minimumAttackInterval);

        Debug.Log($"Magic Bolt attack speed improved. Interval: {attackInterval}");
    }

    public void ImproveAttackRange(float bonusRate)
    {
        if (!isUnlocked)
            return;

        if (bonusRate <= 0f)
            return;

        attackRange *= 1f + bonusRate;
        attackRange = Mathf.Min(attackRange, maximumAttackRange);

        Debug.Log($"Magic Bolt range improved. Range: {attackRange}");
    }

    private Transform FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange,
            enemyLayer
        );

        Transform nearestEnemy = null;
        float nearestDistanceSqr = float.MaxValue;

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyHealth enemyHealth = hits[i].GetComponent<EnemyHealth>();

            if (enemyHealth == null)
                continue;

            float distanceSqr = ((Vector2)hits[i].transform.position - (Vector2)transform.position).sqrMagnitude;

            if (distanceSqr < nearestDistanceSqr)
            {
                nearestDistanceSqr = distanceSqr;
                nearestEnemy = hits[i].transform;
            }
        }

        return nearestEnemy;
    }

    private void Fire(Transform target)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Magic Bolt projectile prefab is not assigned.");
            return;
        }

        Vector3 spawnPosition = projectileSpawnPoint != null
            ? projectileSpawnPoint.position
            : transform.position;

        Vector2 direction = target.position - spawnPosition;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        MagicBoltProjectile projectile = GetOrCreateProjectile();

        if (projectile == null)
        {
            Debug.LogWarning("MagicBoltProjectile component is missing on projectile prefab.");
            return;
        }

        projectile.transform.position = spawnPosition;
        projectile.transform.rotation = Quaternion.identity;
        projectile.gameObject.SetActive(true);
        projectile.Initialize(direction, damage, enemyLayer);
    }

    public void ReturnProjectileToPool(MagicBoltProjectile projectile)
    {
        if (projectile == null)
            return;

        if (!useProjectilePooling)
        {
            Destroy(projectile.gameObject);
            return;
        }

        projectile.gameObject.SetActive(false);
        projectilePool.Enqueue(projectile);
    }

    private void PrewarmProjectilePool()
    {
        if (projectilePrefab == null)
            return;

        for (int i = 0; i < projectilePoolPrewarmCount; i++)
        {
            MagicBoltProjectile projectile = CreateProjectileInstance();

            if (projectile == null)
                continue;

            projectile.gameObject.SetActive(false);
            projectilePool.Enqueue(projectile);
        }
    }

    private MagicBoltProjectile GetOrCreateProjectile()
    {
        if (!useProjectilePooling)
        {
            return CreateProjectileInstance();
        }

        if (projectilePool.Count > 0)
        {
            return projectilePool.Dequeue();
        }

        return CreateProjectileInstance();
    }

    private MagicBoltProjectile CreateProjectileInstance()
    {
        if (projectilePrefab == null)
            return null;

        GameObject projectileObject = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
        MagicBoltProjectile projectile = projectileObject.GetComponent<MagicBoltProjectile>();

        if (projectile == null)
        {
            Destroy(projectileObject);
            return null;
        }

        projectile.SetOwner(this);
        return projectile;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
```

### FILE: Assets/_Project/Scripts/Weapons/PlayerMeleeAutoAttack.cs

```csharp
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerMeleeAutoAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackInterval = 0.8f;
    [SerializeField] private float attackRange = 1.15f;
    [SerializeField] private float attackRadius = 0.55f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Critical")]
    [SerializeField] private float criticalChance = 0f;
    [SerializeField] private float criticalMultiplier = 2f;
    [SerializeField] private float maximumCriticalChance = 0.5f;

    [Header("Relic Bonus")]
    [SerializeField] private float relicAttackSpeedBonus = 0f;

    [Header("Limit")]
    [SerializeField] private float minimumAttackInterval = 0.2f;
    [SerializeField] private float maximumAttackRange = 3.5f;
    [SerializeField] private float maximumAttackRadius = 2.0f;

    [Header("Visual")]
    [SerializeField] private GameObject attackVisual;
    [SerializeField] private float visualDuration = 0.12f;

    private PlayerController playerController;
    private float attackTimer;
    private Coroutine visualCoroutine;

    public int Damage => damage;
    public float AttackInterval => attackInterval;
    public float AttackRange => attackRange;
    public float AttackRadius => attackRadius;
    public float CriticalChance => criticalChance;
    public float RelicAttackSpeedBonus => relicAttackSpeedBonus;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        if (attackVisual != null)
        {
            attackVisual.SetActive(false);
        }
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = GetCurrentAttackInterval();
        }
    }

    public void AddDamage(int amount)
    {
        if (amount <= 0)
            return;

        damage += amount;

        Debug.Log($"Weapon damage increased. Damage: {damage}");
    }

    public void ImproveAttackSpeed(float rate)
    {
        if (rate <= 0f)
            return;

        attackInterval *= 1f - rate;
        attackInterval = Mathf.Max(minimumAttackInterval, attackInterval);

        Debug.Log($"Attack speed improved. Interval: {attackInterval}");
    }

    public void ImproveAttackRange(float rate)
    {
        if (rate <= 0f)
            return;

        attackRange *= 1f + rate;
        attackRadius *= 1f + rate;

        attackRange = Mathf.Min(maximumAttackRange, attackRange);
        attackRadius = Mathf.Min(maximumAttackRadius, attackRadius);

        Debug.Log($"Attack range improved. Range: {attackRange}, Radius: {attackRadius}");
    }

    public void AddCriticalChance(float amount)
    {
        if (amount <= 0f)
            return;

        criticalChance += amount;
        criticalChance = Mathf.Min(criticalChance, maximumCriticalChance);

        Debug.Log($"Critical chance increased. Critical Chance: {criticalChance * 100f}%");
    }

    public void SetRelicAttackSpeedBonus(float bonusRate)
    {
        relicAttackSpeedBonus = Mathf.Max(0f, bonusRate);
    }

    private float GetCurrentAttackInterval()
    {
        float finalInterval = attackInterval;

        if (relicAttackSpeedBonus > 0f)
        {
            finalInterval *= 1f - relicAttackSpeedBonus;
        }

        return Mathf.Max(minimumAttackInterval, finalInterval);
    }

    private void Attack()
    {
        if (playerController == null)
            return;

        Vector2 facingDirection = playerController.FacingDirection.normalized;
        Vector2 attackCenter = (Vector2)transform.position + facingDirection * attackRange;

        ShowAttackVisual(facingDirection);

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackCenter,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                int finalDamage = CalculateDamage();
                enemyHealth.TakeDamage(finalDamage, transform.position);
            }
        }

        Debug.DrawLine(transform.position, attackCenter, Color.red, 0.2f);
    }

    private int CalculateDamage()
    {
        bool isCritical = Random.value < criticalChance;

        if (!isCritical)
        {
            return damage;
        }

        int criticalDamage = Mathf.RoundToInt(damage * criticalMultiplier);

        Debug.Log($"Critical Hit! Damage: {criticalDamage}");

        return criticalDamage;
    }

    private void ShowAttackVisual(Vector2 direction)
    {
        if (attackVisual == null)
            return;

        attackVisual.transform.localPosition = direction * attackRange;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackVisual.transform.localRotation = Quaternion.Euler(0f, 0f, angle);

        if (visualCoroutine != null)
        {
            StopCoroutine(visualCoroutine);
        }

        visualCoroutine = StartCoroutine(AttackVisualRoutine());
    }

    private IEnumerator AttackVisualRoutine()
    {
        attackVisual.SetActive(true);

        yield return new WaitForSeconds(visualDuration);

        attackVisual.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 direction = Vector2.right;

        PlayerController controller = GetComponent<PlayerController>();

        if (controller != null)
        {
            direction = controller.FacingDirection.normalized;
        }

        Vector2 attackCenter = (Vector2)transform.position + direction * attackRange;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCenter, attackRadius);
    }
}
```

