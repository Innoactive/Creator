# Localization

Localization can be used to allow courses to run in different languages. Available Languages can be selected during runtime by using the trainer menu.

![How to change language during runtime](../images/localization/menu-languages.png)

## Where is localization used?

Localization is mainly used for text to speech audio during a training course. But it can also be used to choose which already existing audio file should be played and for specific customization for the desktop mode.

Following default behaviors can use localization:
- [Play Text to Speech](./default-behaviors.md#audioplay-texttospeech-audio) 
- [Play Audio File](./default-behaviors.md#audioplay-audio-file)

To use localization you will have to add a localization key:

![How to change language during runtime](../images/localization/key-mapping.png)

## Add a new language

Data for localization is stored in JSON files which are by default stored at `Assets/StreamingAssets/Training/{Course Name}/Localization` and are named after the ISO-code of the languages, for example EN.json, DE.json, ES.json etc.

The JSON file contains a list of key/value pairs consisting out of strings, for example an english language file:
```json
{
    "grab_sphere": "Please, grab the sphere using the side button of your controller.",
    "put_sphere": "Please, move the sphere closer to the cube.",
    "move_cube": "Behold! The mighty flying cube!",
    "training_complete": "Congratulations! The training is complete."
}
```

## Extend language files for specific cases

Using Standalone or Desktop Mode may require customized translations. For example in Desktop Mode you want to change the audio instruction 'grab the sphere using the side button of your controller.' to 'grab the sphere by clicking the object with your left mouse key'. To do so there is the possibility to add an additional language file extension which overwrites existing keys in the original file. Only changed keys will be overwritten. 

An extension for an english translation would look like this:

![Languages file structure](../images/localization/language-files.png)

When starting the training in Desktop Mode first the EN.json will be loaded and after that the EN_desktop_mode.json which will extend the already existing key/value pairs and overwrite duplicates.

***Be aware that an base file like EN.json always have to exist to make the language selectable.***

