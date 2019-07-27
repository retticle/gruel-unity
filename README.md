```
   _____   _____    _    _   ______   _      
  / ____| |  __ \  | |  | | |  ____| | |     
 | |  __  | |__) | | |  | | | |__    | |     
 | | |_ | |  _  /  | |  | | |  __|   | |     
 | |__| | | | \ \  | |__| | | |____  | |____ 
  \_____| |_|  \_\  \____/  |______| |______|
```

# About
**Gruel is:**
* A collection of resources and systems needed for building games in Unity.
* Highly experimental (for now).
* Built to be easily added as a submodule into Unity projects.

For now Gruel's development and versioning is based around what I need for my current projects, and when they are completed. As such it may be lacking obvious features you would want for a different project, but I didn't have a need for in mine.

## **Version**
**Current version:** `0.1.0`  
**Version formatting:** `MAJOR.MINOR.PATCH`

* Each release of Gruel will be branched off of master, and the `MINOR` version number will be incremented.
* Breaking changes should only come with each `MINOR` increment.
* `PATCH` releases should always be safe.

# What does Gruel contain?
## **Index:**
* [Actor](#actor)
* [Audio](#audio)
* [Camera](#camera)
  * [CameraController](#cameracontroller)
    * [CameraAspectUtility](#included-camera-traits)
    * [CameraAttachables](#included-camera-traits)
    * [CameraOrthographicScalar](#included-camera-traits)
    * [CameraPanner](#included-camera-traits)
    * [CameraShake](#included-camera-traits)
    * [CameraTracker](#included-camera-traits)
* CoroutineSystem
  * [CoroutineRunner](#coroutinerunner)
  * [ManagedCoroutine](#managedcoroutine)
* [Flipbooks](#flipbook)
  * [SpriteFlipbook](#flipbook)
  * [ImageFlipbook](#flipbook)
  * [SpriteFlipbookData](#flipbook)
* [FlowMachine](#flowmachine)
* [Localization](#localization)
* [ObjectPool](#objectpool)
* [StateMachine](#statemachine)
* [Widget](#widget)
* [VariableObjects](#variableobjects)