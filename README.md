This project explores game feel and extensible architecture. Along the way I applied SOLID principles, state machines, data-driven design, the observer pattern, and interfaces — aiming for weighty, fluid combat that holds its pace.

Movement and combat run on two concurrent state machines rather than one, since an entity can  only occupy a single state within either machine at a time. New states derive from a shared base class, keeping the two systems independent unless explicit interaction is needed — a sprint speed penalty during an attack, for example.

Locomotion uses a CharacterController with SmoothDamp-based acceleration tied to speed and weight, paired with a weight-scaled camera shake and an intentional camera-follow delay to sell momentum.

Combat splits into three phases — Windup, Active, Recovery — driven by animator speed on a single clip. Extending Windup makes both player combos and enemy attacks readable and dodgeable. A Hit React state interrupts combos on damage, briefly dropping movement speed for consistent timing. Active-phase hits use a Physics.OverlapBox against IDamagable targets, tracked in a HashSet to block multi-hits per swing and cleared on exit. Successful hits trigger a red flash material, a scale pulse, an animator speed dip, and impact VFX.

Enemy AI runs on lightweight patrol and detection loops: enemies wander random points in a spawn-defined area, re-targeting on arrival. Detection checks distance before angle to skip unnecessary trig each frame. Entering the vision cone triggers a chase; taking damage alerts the enemy even from outside it. Enemy spawning uses object pooling — enemies reset and deactivate on death instead of being destroyed.

Skills are ScriptableObject-driven; instant and continuous types run through coroutines, and adding a new one requires only a new asset. Buff and debuff values are applied directly to the StatManager's stat fields, with cooldown routines automatically reversing the effect on expiry.
