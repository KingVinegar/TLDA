
public partial class VB25dTKEditor {

    public void StrHelp(int index) {
        switch (index) {
            case 0:
                stringMsg = "2.5D Toolkit allow to easily create and manage a 2.5d environments for your games " +
                    "but it can also be used simply to create meshes of any shape and complexity.\n";
                break;
            case 1:
                stringMsg = "<Show areas labels> Show/Hide areas numbers.\n" +
                    "<Show character> Show/Hide character.\n" +
                    "<Use character resize> Available in Orthographic projection. It allows to assign two different scales " +
                    "to the character at two different coordinates.\n" +
                    "<Preview resize> Working with the advanced tools of 2.5D Toolkit: while moving the character with the mouse " +
                    "it will automatically be resized to get a preview.\n" +
                    "<Spot size> Adjusts spots size (area vertices).\n" +
                    "<Draw wire spot> Make spots transparent.\n" +
                    "<Enable accuracy> Available for some fields. Increase accuracy for placing and resizing objects.\n" +
                    "<Enable hold down> Allows to keep down some buttons without having to click them several times to move objects faster.\n" +
                    "<View camera> Hide/Show camera frustum.";
                break;
            case 2:
                stringMsg = "<Cursor> You can use different cursor (2.5dToolKit/Cursor folder) or the default one leaving the field empty.\n" +
                   "<Empty cursor> Empty Cursor field.\n" +
                   "<Active area> Editable area.\n" +
                   "<Inactive area> Not editable area.\n" +
                   "<Mouse over spot> Spot color on mouse over.\n" +
                   "<Floor> Floor color.\n" +
                   "With <Label background>, <Label text> and <Label font size> you can change the appearance of the labels (areas numbers).";
                break;
            case 3:
                stringMsg = "Set Alpha channel to about 20% to maintain a good overall view of the background image.";
                break;
            case 4:
                stringMsg = "Using background image in scene you can drag it into its field. This will allow you to set the right camera size/distance " +
                    "according to the projection used.\n" +
                    "or\n" +
                    "You can create a second camera that renders the background. Drag image into its field.\n" +
                    "This will allow you to set the right camera size / distance according to the projection used.\n" +
                    "To enable this option drag the Main Camera into its field.";
                break;
            case 5:
                stringMsg = "This option allows you to build your environment when it requires only one floor (not multilevel) without using a physical Plane.\n" +
                    "Yellow line represents the horizon. When it turns red it indicates that you are on the wrong side of the floor.\n" +
                    "<Start/End work> Enable/disable this tool.\n" +
                    "<Initialize environment> Reset the environment to its initial working state.\n" +
                    "<Floor position> Being the work floor infinite it will be useful only to move it along the Y axis.\n" +
                    "<Show floor> Hide/Show floor.\n" +
                    "<Dimension> Change the number of squares displayed. Floor dimension is always infinite.\n" +
                    "<Reset floor> Set floor to -1 on the Y axis.\n" +
                    "<Camera off> Turn off/on camera.\n" +
                    "<Orthographic> Change camera projection.\n" +
                    "<Position> and <Rotation> set camera values.\n" +
                    "<Disable> Enable/Disable automatic camera distance setting dependent on the FOV.\n" +
                    "<FOV> Set FOV. If the automatic distance setting option is not disabled, the camera's Z axis will automatically change based on this value.\n" +
                    "<Reset Camera> Set camera position to (0, 1, -10) and Rotation to (3, 0, 0).\n" +
                    "<Apply camera distance> or <Apply camera size> Generally they are not needed as these values are automatically applied.\n" +
                    "<Image> Not needed. See manual.\n" +
                    "<Position>, <Rotation>, <Scale> set character values.\n" +
                    "<Lock scale> Allow to scale character on the three axes simultaneously.\n" +
                    "<Reset character> Set character position to(0, 0, 0), Rotation to(0, 0, 0) and Scale to(1, 1, 1).\n" +
                    "<Use cube> Cube is useful for trying to guess FOV and camera rotation.\n" +
                    "<Place cube> Move Cube with left mouse button. Since the same button is used by the Meter tool, activating one disables the other and vice versa.\n" +
                    "<Transparent cube> Makes cube semi-transparent to better overlay it on objects.\n" +
                    "<Don't keep proportion> Useful in perspective projection.\n" +
                    "<Position>, <Rotation>, <Scale> Set Cube values.\n" +
                    "<Reset Cube> Set Cube scale to 1.\n" +
                    "<Enable meter> Allows to measure distances, useful when you know the floor measurements or when you can hypothesize them.\n" +
                    "<Automatic meter> Automatically updates measurements according to new scene settings.\n" +
                    "<Show label> Show/Hide label at the top left of the screen.\n" +
                    "To set meter in orthographic projection see manual.";
                break;
            case 6:
                stringMsg = "Caution! The vertices or edges of the areas must not overlap.\n" +
                    "<Create new area> Initializes a new area to draw.\n" +
                    "<Draw in scene view> Enables drawing in Scene view (Available only if the <Use 2.5D Toolkit environment> option is not used).\n" +
                    "<Current area> Increments for each new area created and allows you to select the area to work on.\n" +
                    "You can delete an area by clicking on the trash icon.\n" +
                    "<Type> Defines whether the area is walkable or non-walkable.\n " +
                    "<Spot radius> Sets the response radius of the spot on mouseover.\n" +
                    "<Line radius> Sets the response radius of the edge of the area on mouseover.\n" +
                    "<Show line degrees> Show line angle on 360 ° and every 90 ° useful for quickly finding angles such as 30/45/60.\n" +
                    "<Show label> Show a summary label which follows the mouse or fixed at the top. It can be resized.\n" +
                    "<Disable function> Middle mouse button will not delete the last inserted vertex. " +
                    "Useful when you use Game View zoom.\n" +
                    "<Keep line straight> This option is visible only with Tilemap or environments set in orthographic projection." +
                    "Allows to draw a straight horizontal or vertical line. Mouse distance from line can be changed.\n" +
                    "<Remove last Point> Delete the last vertex entered in the selected area. " +
                    "Middle mouse button allows same operation.\n" +
                    "<Remove ALL Point> Delete all the inserted vertices of the selected area.\n" +
                    "<Close perimeter> It joins the first to the last vertex by completing the area. To close the perimeter you can also overlap " +
                    "the two vertices while you are drawing or move one vertex on the other.\n" +
                    "<Select point> Allows to select a vertex and move it through buttons.\n" +
                    "<Destroy all areas>  Remove all areas.\n" +
                    "<Save areas data> Save created areas in 2.5d Toolkit/VBDataAreaTK/[Scene Name] folder. Each save will overwrite the previous one.\n" +
                    "<Load areas data> Restore saved areas.";
                break;
            case 7:
                stringMsg = "This option will only be available if at least one area has been created.\n" +
                    "Caution! The vertices or edges of the areas must not overlap.\n" +
                    "<Mesh color> Assign a color to the mesh before it is created.\n" +
                    "<Create mesh> Mesh will be created by default double-sided. " +
                    "With <Double-sided> and <Reverse> you can switch from one mode to another. If the mesh is not visible in <Reverse> mode " +
                    "simply click the same button again.\n" +
                    "<Destroy mesh> Clears the mesh.\n" +
                    "<Save as> You can name the mesh to save. By default it will be '[name scene]Mesh'.\n" +
                    "<Save mesh> The mesh will be deleted from Hierarchy and saved in 2.5dToolKit/Resources/[scene name] folder in two formats: " +
                    ".asset and .prefab. \n" +
                    "<Don't destroy on save> This option is used in conjunction with 'Save as'. " +
                    "Normally when you save the mesh it will be removed from the Hierarchy. Checking this box the mesh will not be deleted. " +
                    "By keeping the mesh in the scene you can use it as a reference to design another area.";
                break;
            case 8:
                stringMsg = "<Hide mesh> Hide/Show mesh (only the default one called VBMeshTK).\n" +
                    "<Hide areas> Hide/Show all areas and Floor.\n" +
                    "<Hide path line> Hide/Show character path line.\n" +
                    "Note: available only in Play mode.";
                break;
            case 9:
                stringMsg = "<Object> Drag from Hierarchy a scene object here. It can be a 2d or 3d object. \n" +
                    "<Empty object> Empty Object field. \n" +
                    "<Hide object> You can hide/show object. " +
                    "Especially useful with 2D objects to ensure that they are perfectly overlapped to the background image from which they " +
                    "were cut out. If there are no differences the sprite size is right.\n" +
                    "<Reload object> Using this button you can delete 2D or 3D object from the Hierarchy and load it again from its folder with its original size.\n" +
                    "<Auto position> To be used with sprites of the same size as the background image. Note: pivot must be set to Center. See manual and videos.\n " +
                    "<Position>, <Rotation>, <Scale> Set object values.\n" +
                    "<Lock scale> Allow to scale object on the three axes simultaneously.\n" +
                    "<+ 0.5>, <- 0.5> If the object is very large or very small it may be useful to use +5 and -5 to resize it more quickly. \n" +
                    "<Copy scale> If for some reason you are not using the automatic Size/Camera distance setting you may want to use a certain scale for all objects. " +
                    "Once you set your personal scale on an object copy it and assign it to all other objects with <Assign scale>.\n" +
                    "<Assign cam rotation> Assign camera rotation values to the object. Will be done automatically on 2D objects when you drag it into its field.\n" +
                    "<Hide pivot> Hide/Show sprites pivot.\n" +
                    "<Hide areas> Hides all areas/mesh to free view and better place objects. The mesh must be the default one called VBMeshTK.\n" +
                    "<Don't keep proportions> Enabled in Perspective projection. Normally the object keep its proportions by moving it " +
                    "along the Z axis, selecting this option the object will change size according to the distance from the camera. \n";
                break;
            case 10:
                stringMsg = "Be careful: \n" +
                    "<Destroy all areas> Will remove every areas in the scene.";
                break;
            case 11:
                stringMsg = "<Export all data> Save a file containing values of each object into Hierarchy and areas vertices. Make sure objects are visible in Hierarchy.";
                break;
            case 12:
                stringMsg = "Never move background image, it will always be restored to its default position. Once your environment is complete, you can move it to any location.";
                break;
            case 13:
                stringMsg = "Move whole environment to another location\n" +
                            "Be carefull:\n" +
                            "Do this only when the scene is complete!\n" +
                            "Scene can no longer be edited through the 2.5D Toolkit tools. Areas will be removed.";
                break;
            case 14:
                stringMsg = "Drag Grid and Tilemap into their fields to create areas.";
                break;
            case 15:
                stringMsg = "To use Unity Navigation, environment need to be rotated 90° before starting creating areas.\n" +
                            "Uncheck to restore default rotation.";
                break;
            case 16:
                stringMsg = "Use Auto position with sprites equal in size to the background image.";
                break;
        }
    }
}

