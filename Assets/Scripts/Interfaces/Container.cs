public abstract class Container : Interactable
{
    public enum ContainerTypes { BUCKET, BOTTLE, CAULDRON, BOWL, DRAIN, BIN };
    public int MaxCapacity;

    public ContainerTypes thisType;
    public abstract bool EmptyContent(ContainerFiller item);
    public abstract bool AddToContainer(ContainerFiller item);
    public abstract ContainerFiller[] GetContents();
    public abstract bool IsEmpty();
    public abstract bool IsFull();


    public static bool MoveContents(Container hand, Container target)
    {
        if(hand.IsEmpty() && target.IsEmpty())
        {
            //nada
            return false;
        }
        if (hand.IsEmpty())
        {
            // Drain from target to Hand
            if(hand.thisType == ContainerTypes.BUCKET)
            {
                // Bucket is a special case it basically is used to tip everything in/out
                ContainerFiller[] targetContents = target.GetContents();
                for (int i = 0; i < targetContents.Length; i++)
                {
                    if (targetContents.Length <= 0)
                    {
                        //what?
                        return false;
                    }
                    if (!hand.AddToContainer(targetContents[i]))
                    {
                        // failed to add it over
                        return false;
                    }
                    target.EmptyContent(targetContents[i]);
                }
            }
            else
            {
                ContainerFiller[] targetContents = target.GetContents();
                if(targetContents.Length <= 0)
                {
                    //what?
                    return false;
                }
                if(!hand.AddToContainer(targetContents[0]))
                {
                    // failed to add it over
                    return false;
                }
                target.EmptyContent(targetContents[0]);
            }
        }
        else
        {
            // Put from hand to target
            if (hand.thisType == ContainerTypes.BUCKET)
            {
                // Bucket is a special case it basically is used to tip everything in/out
                ContainerFiller[] handContents = hand.GetContents();
                for (int i = 0; i < handContents.Length; i++)
                {
                    if(target.AddToContainer(handContents[i]))
                    {
                        hand.EmptyContent(handContents[i]);
                    }
                    else
                    {
                        // not likey so stoppy
                        break;
                    }
                }
            }
            else
            {
                ContainerFiller[] handContents = hand.GetContents();
                if(handContents.Length <= 0)
                {
                    // nothing? what?
                    return false;
                }
                if (target.AddToContainer(handContents[0]))
                {
                    hand.EmptyContent(handContents[0]);
                }
            }
        }

        return true;
    }

    private void Start()
    {
        container = true;
        usedOnWorldObject = true;
    }
}
