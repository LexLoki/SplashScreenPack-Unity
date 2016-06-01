# SplashScreenPack-Unity
Unity pack with C# script for splash screen animation

![alt tag](https://cloud.githubusercontent.com/assets/9408934/15724244/a0d48da8-281c-11e6-8458-78cbd239c9c6.png)

Simple package with a scene that represents a splash screen for your game, with ready to go fadeIn and fadeOut animations.
* The scene is basically a "SplashScreen" GameObject representing the camera and containing a SplashSprite GameObject to render the sprite and to contain the script.
* To use your own sprite for the background, drag and drop it to the "Sprite" field of the "Sprite Renderer" component of the "SplashSprite" GameObject.
* From "SplashSprite", you can customize the script from the inspector, with time durations for the fadeIn and fadeOut, and also in-between phase.
* You also can and should specify a scene, by name or index, for the script to load after it ends the animation.
* Don't worry about the size of the sprite not fitting the camera. The script automatically scales it to fit it the best way without stretching it.
* The checkbox scaleToFill should be marked if you want to force the sprite to cover whole camera, even if it needs to stretch it to be able to.
