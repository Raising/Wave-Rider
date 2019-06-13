public interface ISelectableObject<T>
{
    VerificableAction RequestSelection(Resolutor<T> completeSelection);
}