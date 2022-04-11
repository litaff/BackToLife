namespace BackToLife
{
    public class Block : Entity
    {
        private BlockType _type;
    
        private void Awake()
        {
            _type = BlockType.Regular;
        }
    
    
    }
}