mkdir 'Innoactive'
mkdir 'Innoactive/Creator'
mkdir 'Innoactive/Creator/Components'

cd 'Innoactive/Creator'

git submodule add git@github.com:Innoactive/Creator.git Core

git submodule add git@github.com:Innoactive/VRTK-Interaction-Component.git 'Components/VRTK-Interactions'
git submodule add git@github.com:Innoactive/TextToSpeech-Component.git 'Components/Text-To-Speech'

git submodule update --init --recursive
