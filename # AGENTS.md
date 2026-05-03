# AGENTS.md

## Project Identity

Project name: 2D Dungeon Survivor  
Genre: 2D side-view survival roguelike / Vampire Survivors-like  
Engine: Unity  
Development standard: production-oriented, not throwaway prototype  
Visual direction: pixel-art dark fantasy, Stoneshard-like reference  
Current target: stabilize 5-minute MVP core loop, then expand to 10-minute vertical slice

---

## Core Development Principles

1. Build one feature at a time.
2. Do not rewrite working systems without clear reason.
3. Preserve existing project structure.
4. Prefer simple, stable, beginner-maintainable Unity/C# code.
5. Avoid over-engineering, unnecessary abstraction, and premature optimization.
6. Every code change must state:
   - target file
   - file path
   - reason for change
   - full replacement code or exact modified section
   - Unity Inspector connection steps
   - expected result after Play
7. Do not invent missing project details. If unsure, ask for the relevant file or state uncertainty.
8. Prioritize a playable, understandable, expandable structure.

---

## Response Style for the Assistant

- Use Korean.
- Use a formal, professional, direct tone.
- Be concise but do not omit necessary steps.
- Explain only what is needed for the current step.
- Do not repeat installation or already completed setup.
- Do not jump ahead to unrelated systems.
- Always proceed step by step.

---

## Current Project Structure

Use this folder structure as the standard:

```text
Assets/_Project
├── Animations
├── Art
├── Audio
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
    ├── Relics
    ├── Spawning
    ├── UI
    └── Weapons