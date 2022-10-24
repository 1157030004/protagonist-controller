# Protagonist Controller
A physic based character controller inspired by Genshin Impact & Zelda BTOW.

## Architecture
This system is following Hierarchical FInite State Machine (HFSM) design pattern to orchestrate every states.

### Floating Capsule
By giving upward force to capsule collider, the protagonist will be able to move in many complex ground structure such as: slope, stairs and niche.

### Input System
New Input system  is used to handle two different inputs: Mouse & keyword and Gamepad

### Camera Movement
By using Cinemachine, the camera is able to behave in number of behaviors:
[x] Colliding with environment
[x] Automatic recentering
[x] Zoom in & out

### Grounded Movement
In grounded movement, the protagonist is able to:
[x] Idling
[x] Walking
[x] Running
[x] Dashing
[x] Sprinting
[x] Rolling
[x] Landing
It also includes variation of animation transition between states

### Airborne Movement
In airborne movement, the protagonist is able to"
[x] Falling
[x] Jumping
[x] Gliding

## Upcoming Features
[] Swimming system
[] Diving system
[] Climbing system