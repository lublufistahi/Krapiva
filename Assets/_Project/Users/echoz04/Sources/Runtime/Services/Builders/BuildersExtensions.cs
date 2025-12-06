using Sources.Runtime.Gameplay.Character;

namespace Sources.Runtime.Services.Builders
{
    public static class BuildersExtensions
    {
        public static CharacterRoot ProvideData(this CharacterRoot characterRoot, CharacterData data)
        {
            characterRoot.SetData(data);
            return characterRoot;
        }
        
        public static CharacterRoot ProvideInput(this CharacterRoot characterRoot, CharacterInput input)
        {
            characterRoot.SetInput(input);
            return characterRoot;
        }
        
        public static CharacterRoot Initialize(this CharacterRoot characterRoot)
        {
            characterRoot.InitializeSystems();
            return characterRoot;
        }
    }
}