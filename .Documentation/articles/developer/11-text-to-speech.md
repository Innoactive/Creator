# Text to Speech Engine (TTS)

When you create a training, one of the most time consuming steps is managing the audio. Instructions are changed quite often to keep them up to date with a current version of a training. If you use a live actor to record the instructions, you have to do the recordings over and over again every time the instructions are changed. One of the alternative solutions is to use a text to speech synthesizer that will generate voice records automatically. 

The Innoactive Creator provides a rich support for different text to speech engines. There is support for both offline and online text to speech engines with optional caching to save the bandwidth. It is also possible to modify or replace the spoken text after the training application was compiled. This allows to speed up the development process even further.

Supported text to speech engines:

* [Windows](#microsoftsapitexttospeechprovider) (offline)
* [Watson](#watsontexttospeechprovider) (online)
* [Google](#googletexttospeechprovider) (online)

There is also an option to integrate additional text to speech engines.

## How to Setup a Text to Speech Engine (TTS)
The configuration is stored in a scriptable object called TextToSpeechConfiguration which is accessible via the Innoactive menu `Innoactive > Creator > Windows > TextToSpeech Settings`. It is useable out of the box and configured to use the windows text to speech API which allows to create audio files on any windows 10 machines without connection to the internet.
 
To play a text to speech audio use the `Play TextToSpeech Audio` behavior which is a behavior provided by the TextToSpeech component. Adding a text in the default text field is enough to get an audio line generated.
 
Handling multiple languages can be done by using the `Localization` feature. The different translations are stored as key value pairs within a json file. The `Play TextToSpeech Audio` behavior will use the text matching the localization key. The translation files have to be place inside `StreamingAsset/YourCourse/Localization/` folder. When using the Innoactive Template the naming scheme for these files should be the first two letters of the iso country codes for example EN.json, DE.json or ES.json

Example EN.json file:
```json
{
    "grab_sphere": "Please, grab the sphere using the side button of your controller.",
    "put_sphere": "Please, move the sphere closer to the cube.",
    "move_cube": "Behold! The mighty flying cube!",
    "training_complete": "Congratulations! The training is complete."
}
```

[![Play TextToSpeech Audio example](../images/developer/play-text-to-speech-example.png)](../images/developer/play-text-to-speech-example.png "A play audio behavior configured to use a localization key")

### TextToSpeechConfig Configuration

The text to speech configuration contains following parameter

Parameter | Type | Default Value | Required | Description
--- | --- | --- | --- | ---
Language | String | EN | Yes | Language which should be used, depends on the chosen provider.
Voice | String | Male | Yes | Voice that should be used, depends on the chosen provider.
UseCache | Boolean | true | No | Will cache the audio files to prevent redownloading it everytime.
UseStreamingAssetFolder | Boolean | true | No | Will use the StreamingAsset folder as additional place to keep the cached audio files.
SaveAudioFilesToStreamingAssets | Boolean | false | No | Triggers the engine to store the created audio files in the StreamingAsset folder, used for creating the default audio files.
StreamingAssetCacheDirectoryName | String | "TextToSpeech" | No | Subfolder of the StreamingAsset path used to store the files.
Auth | String | null | No | Used to authenticate at the provider, if required.
Provider | DropDown | MicrosoftSapiTextToSpeechProvider | Yes | Provider used `GoogleTextToSpeechProvider`, `WatsonTextToSpeechProvider`, `MicrosoftSapiTextToSpeechProvider`

#### MicrosoftSapiTextToSpeechProvider

MicrosoftSapiTextToSpeechProvider uses the in Windows integrated text to speech api which is available for Windows 10. Also to be able to be used the specific language pack has to be installed on Windows.

Parameter | Description
--- | ---
Language | The Language value has to be a Two-letter country code for example EN, DE or ES (see full list [here](https://en.wikipedia.org/wiki/ISO_3166-1))
Voice | The voice can be Male, Female or Neutral

#### WatsonTextToSpeechProvider

The Watson text to speech service is a paid service which can be found here: [Watson service](https://www.ibm.com/watson/services/text-to-speech/). To be able to use it an account is required.

Parameter | Description
--- | ---
Auth | Add your access token you received from Watson.
Language | Language field depends on the voice used, if for example `en-US_LisaV2Voice` is used, the `en-US` part goes here.
Voice | Used voice for the `en-US_LisaV2Voice` example the `LisaV2Voice` will be put in here. To see all voices available check [List of voices](https://cloud.ibm.com/apidocs/text-to-speech#list-voices)

Be aware that the config file can be accessed in the compiled version of your product. Your token is **NOT PROTECTED** from misuse.

#### GoogleTextToSpeechProvider

We still use Google text to speech API version 1 which is kind of outdated but still working. This provider **might be depricated** sooner or later. Till then you can still use it.

Parameter | Description
--- | ---
Language | Two-letter country code for example EN, DE or ES
Voice | Stopped working as expected, will stay female for now

[To the next chapter!](12-dependency-manager.md)