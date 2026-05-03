# DevState.md

## 프로젝트 개요

- 프로젝트명: 2D Dungeon Survivor
- 장르: 2D 가로형 생존 로그라이크 / 뱀서류
- 개발 기준: 정식 출시 개발 기준
- 그래픽 방향: 도트 기반 다크 판타지, 스톤샤드풍 참고
- 현재 목표: 5분 동안 반복 플레이 가능한 핵심 루프 완성

---

## 현재 완료된 기능

### 플레이어
- 플레이어 이동 가능
- 기사 도트 스프라이트 적용 완료
- Player Pivot을 발밑 기준으로 조정
- Player Collider 수동 조정 진행
- Rigidbody2D Gravity Scale 0 사용

### 전투
- 자동 근접 공격 존재
- 몬스터 피격 가능
- 몬스터 넉백 가능
- 피격 시 색 변화 및 순간 스케일 변화 적용
- 타격감은 최소 수준 완료, 세부 polish는 추후 작업

### 몬스터
- 기존 빨간 박스 Enemy 프리팹 존재
- 구울 프리팹 추가 완료
- 박쥐 프리팹 추가 완료
- EnemySpawner에서 몬스터 Spawn Table 사용
- 몬스터별 Spawn Weight 비율 설정 가능
- 기존 빨간 박스 Enemy는 Spawn Table에서 제외하면 더 이상 스폰되지 않음

### 경험치 / 성장
- ExpGem 드랍 및 획득 존재
- PlayerExp 시스템 존재
- 레벨 개념 존재
- 레벨업 선택 시스템은 다음 우선순위로 정식화 필요

### UI
- HUDCanvas 존재
- HP Bar, EXP Bar, Wave Text 표시
- StartMenuCanvas 생성 완료
- GAME START 버튼으로 게임 시작 가능
- GameOverCanvas 생성 완료
- Retry 버튼으로 재시작 가능
- GameFlowManager로 시작 / 게임오버 흐름 관리
- UI 디자인은 아직 임시 상태이며 추후 정리 필요

### 맵 / 배경
- Tilemap 기반 바닥 사용
- StageTileGenerator 존재
- 여러 바닥 타일을 랜덤 배치하는 구조 진행 중
- 현재 타일 퀄리티는 임시/프로토타입 성격이며 나중에 직접 제작한 타일로 교체 예정

---

## 주요 폴더 구조

```text
Assets/_Project
├── Animations
├── Art
│   ├── Characters
│   │   └── Player
│   │       └── Export
│   ├── Enemies
│   │   └── Export
│   └── Tiles
├── Audio
├── Materials
├── Prefabs
│   ├── Enemies
│   ├── Player
│   ├── Projectiles
│   └── UI
├── Scenes
├── ScriptableObjects
├── Scripts
│   ├── Combat
│   ├── Core
│   ├── Data
│   ├── Editor
│   ├── Enemy
│   ├── Items
│   ├── Player
│   ├── Spawning
│   ├── UI
│   └── Weapons
└── Docs