/*
2.5D Toolkit v2.06

Note: 2.5D Toolkit is continuously updated and improved. 
If you have any trouble, need clarification or want new features, write us:
support@vbgamestudio.org

https://www.vbgamestudio.org
https://www.facebook.com/vbgamestudio/

YouTube channel: https://www.youtube.com/channel/UCTyHSaMVnTgCIxAbohdmpwQ

Versions from 2.0 involves countless and remarkable changes.
Look at the manual to know all new options introduced and the many videos available.

****************************************

From version 1.05 you can create multiple walkable areas.
This improvement required a change to the file structure contained in the VBDataAreaTK folder.
If you need to use areas designed with previous versions, do this:
1) Open the text files containing the coordinates of your areas one by one.
2) In the file named as (NameScene)Area1 (which is a walkable area), write "0" (zero) at the beginning.
3) In all other files (which are non-walkable areas) write "1".
Example for walkable area:
	Before
		-20.47138;-1.57;17.47573
		-3.961365;-1.57;-4.652373
		-1.596949;-1.57;-4.676195
		[...]
	After
		0
		-20.47138;-1.57;17.47573
		-3.961365;-1.57;-4.652373
		-1.596949;-1.57;-4.676195
		[...]
See the demo files for clarification.
IF YOU DO NOT APPLY THESE CHANGES BEFORE ENTERING THE PLAY MODE YOUR AREAS MAY BE DELETED.
MAKE A BACKUP BEFORE

****************************************

From version 1.04 you will be able to draw directly in Scene View.
This option is very useful in several cases. 
If your environment is very "deep" you may have some difficulties drawing some areas, eg small areas that cannot be walked.
In this case, after placing your sprites in Game View you can draw your areas around them in Scene View.
It could also happen that you have to draw in areas not visible to the camera. Again this option will be convenient for you.
To move from Game View to Scene View and vice versa click the right mouse button to hide the line and click it again to grab
the line when you are in the other scenario.
Remark: when this option is enabled the mouse buttons will be managed by the script while you are on the Plane.
So to change the view of Scene View make sure you are outside the Plane. 
If the zoom allows you to see only the Plane you need to disable the option to move the view.
This option is visible only if VB25dTK is in "Editing" state.
You can see an example in this video: 
"2.5D Toolkit: how and why to use more meshes in one scene"; video: https://youtu.be/Q6HakB3uk-M

Note: only for Unity 2018.3.0 or higher.
****************************************
From version v1.02 the "Background camera" option has been introduced. You can decide whether to use it or not.
If you use it:
Create a new scene.
Click on <Create background camera> in the "Settings" tab.
A new camera will be created in the Hierarchy. Some settings of the Main Camera will be changed and they will be restored 
if you click on <Delete camera>.
A new layer called VBBackground will be created. Note that it will not be removed if you delete the Background Camera.
Drag the background image into the "Texture" field.
A popup window will suggest you to set a correct aspect ratio in Game View, this is very important to maintain the proportions 
of the sprites cut out from the background image.
Do not use "Fixed Resolution" for images that are a bit large because the gizmos line to build your mesh may not be clear enough.
Now you can create your environment without having to take care of keeping it in front of the background.
If you work with the Background camera in Orthogonal Projection, to have correct sprites proportions place the background image 
in Scene View and set the Main Camera Size accurately overlaying the two images. Then you can delete the image.
This way all your sprites won't need to be resized.
Watch the explanatory '2.5D Toolkit: new Background Camera':
https://www.youtube.com/watch?v=zhmSUqyHCXw
Off course you can ignore these steps and resize all the sprites in Game View.
****************************************

*** Note ***
If the areas are structured on several overlapping levels, they may not be assembled correctly.
In this case it is possible to draw more than one mesh in a scene.
You can see an example in this video: 
"2.5D Toolkit: how and why to use more meshes in one scene"; video: https://youtu.be/Q6HakB3uk-M

When some options are enabled it will not be possible to change tabs.
[tab Navigation]
•	Current state in Editing mode
•	<Hide mesh> enabled
•	<Show character> enabled
[tab Objects]
•	<Object> field not empty
•	<Hide areas> enabled
•	<Enable character> enabled

*** Usage ***
Click Tools> VB Game Studio> 2.5D Toolkit.
Place side by side Scene View and Game View and activate Gizmos in Game View.
Prepare basic environment and enter Play Mode to build your mesh and place 2D/3D objects.
Take a look at manual, demos and videos before starting to use 2.5D Toolkit. 
It is very easy to use but you need to know its main functions.
Script to move character is a basic script. Please click on plane to move character, double click to run.
Click Help icon to view a summary of each available function.


*** Disclaimer ***
Note that in older Unity versions when you load a demo for the first time you may get some errors:
"Unknow shader channel count"
Error will disappear after editing and saving scene.
"Unknown mixed bake mode [...]" 
You can try to solve it by following these steps: Window > Lighting > Settings, click on the gear and reset the settings.
You will have to do this for each demo scene loaded, then save it.
If it doesn't work search forums to solve it.
They are not errors of 2.5D Toolkit.

*/
