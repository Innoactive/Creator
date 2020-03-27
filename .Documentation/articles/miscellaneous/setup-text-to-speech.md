# Text to Speech Engine (TTS)

When you create a training, one of the most time consuming steps is managing the audio. Instructions are changed quite often to keep them up to date with a current version of a training. If you use a live actor to record the instructions, you have to do the recordings over and over again every time the instructions are changed. One of the alternative solutions is to use a text to speech synthesizer that will generate voice records automatically. 

The Innoactive Creator provides a rich support for different text to speech engines. There is support for both offline and online text to speech engines with optional caching to save the bandwidth. It is also possible to modify or replace the spoken text after the training application was compiled. This allows to speed up the development process even further.

Supported text to speech engines:

* [Windows](#microsoftsapitexttospeechprovider) (offline)
* [Watson](#watsontexttospeechprovider) (online)
* [Google](#googletexttospeechprovider) (online)

There is also an option to integrate additional text to speech engines.

## How to Setup a Text to Speech Engine (TTS)

The TTS engine which is used, is declared in the definition of a current training configuration. The `RuntimeConfiguration` looks up for a TTS engine declared in `TextToSpeechConfig` config which should be placed at the configs directory. You may take a look at the `AdvancedTrainingController` in our Innoactive Template to see an example of how to implementent text to speech generation for multiple languages.

To setup your text to speech engine you have to do following steps:

1. Provide a valid `TextToSpeechConfig` with your current implementation of your `IRuntimeConfiguration`. If you use the `DefaultRuntimeConfiguration`, it's enough to create a _text-to-speech-config.json_ in your config folder. If you implement your own version of the `IRuntimeConfiguration`, the easiest way to get a `TextToSpeechConfig` is to load it.
    
    How to load of a `TextToSpeechConfig`:
    ```csharp
    TextToSpeechConfig config = new TextToSpeechConfig();
    if (config.Exists())
    {
        textToSpeechConfig = config.Load();
    }
    ```
2. Provide an `InstructionPlayer` in your `IRuntimeDefinition` which is an AudioSource and will be source of the spoken text.
3. Add a `Play TTS Audio` behavior to your step and configure it with a fitting localization key.
4. Use `Localization` to load your dictionary with all localization keys needed. This dictionary is a json file containing only a list of "key" : "text" values. If you want to be able to change these values during runtime, put those language files into your StreamingAssets folder. 

    Example:
    ```json
    {
        "grab_sphere": "Please, grab the sphere using the side button of your controller.",
        "put_sphere": "Please, move the sphere closer to the cube.",
        "move_cube": "Behold! The mighty flying cube!",
        "training_complete": "Congratulations! The training is complete."
    }
    ```

### TextToSpeechConfig Configuration

The text to speech configuration contains following parameter

Parameter | Type | Default Value | Required | Description
--- | --- | --- | --- | ---
 Provider | String | null | Yes | Name of the provider used, out of the box supported `GoogleTextToSpeechProvider`,`WatsonTextToSpeechProvider`, `MicrosoftSapiTextToSpeechProvider`
Language | String | null | Yes | Language which should be used, depends on the chosen provider.
Voice | String | null | Yes | Voice that should be used, depends on the chosen provider.
Auth | String | null | No | Used to authenticate at the provider, if required.
UseCache | Boolean | true | No | Will cache the audio files to prevent redownloading it everytime.
UseStreamingAssetFolder | Boolean | true | No | Will use the StreamingAsset folder as additional place to keep the cached audio files.
StreamingAssetCacheDirectoryName | String | "TextToSpeech" | No | Subfolder of the StreamingAsset path used to store the files.
SaveAudioFilesToStreamingAssets | Boolean | false | No | Triggers the engine to store the created audio files in the StreamingAsset folder, used for creating the default audio files.

#### MicrosoftSapiTextToSpeechProvider

MicrosoftSapiTextToSpeechProvider uses the in Windows integrated text to speech api which is available for Windows 10. Also to be able to be used the specific language pack has to be installed on Windows.

Parameter | Description
--- | ---
Language | The Language value has to be a Two-letter country code for example EN, DE or ES (see full list [here](https://en.wikipedia.org/wiki/ISO_3166-1))
Voice | The voice can be Male, Female or Neutral


Example:
```json
{
    "Provider": "MicrosoftSapiTextToSpeechProvider",
    "Language" : "EN",
    "Voice": "Female",
}
```

#### WatsonTextToSpeechProvider

The Watson text to speech service is a paid service which can be found here: [Watson service](https://www.ibm.com/watson/services/text-to-speech/). To be able to use it an account is required.

Parameter | Description
--- | ---
Auth | Add your access token you received from Watson.
Language | Language field depends on the voice used, if for example `en-US_LisaV2Voice` is used, the `en-US` part goes here.
Voice | Used voice for the `en-US_LisaV2Voice` example the `LisaV2Voice` will be put in here. To see all voices available check [List of voices](https://cloud.ibm.com/apidocs/text-to-speech#list-voices)


Example:
```json
{
    "Provider": "WatsonTextToSpeechProvider",
    "Auth": "Basic [Your api access token provided by watson]",
    "Language" : "en-US",
    "Voice": "LisaV2Voice",
}
```

Be aware that the config file can be accessed in the compiled version of your product. Your token is **NOT PROTECTED** from misuse.

#### GoogleTextToSpeechProvider

We still use Google text to speech API version 1 which is kind of outdated but still working. This provider **might be depricated** sooner or later. Till then you can still use it.

Parameter | Description
--- | ---
Language | Two-letter country code for example EN, DE or ES
Voice | Stopped working as expected, will stay female for now

Example:
```json
{
    "Provider": "GoogleTextToSpeechProvider",
    "Language" : "en",
    "Voice": "",
}
```
