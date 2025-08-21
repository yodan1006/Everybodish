namespace Grab.Data
{
    public enum MovementStrategyEnum
    {
        None,
        //Hold in front of the character, Grabber governs rotation
        Hold,
        //Drag behind character, draggable governs rotation
        Drag
    }
}
