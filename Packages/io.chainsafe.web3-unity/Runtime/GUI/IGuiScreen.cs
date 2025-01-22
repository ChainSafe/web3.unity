namespace ChainSafe.Gaming.GUI
{
    public interface IGuiScreen
    {
        GuiLayer Layer { get; }

        void OnShowing();
        void OnHiding();
    }
}