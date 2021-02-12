using strange.extensions.context.impl;

public class AppRoot : ContextView
{
    private void Awake()
    {
        context = new MainContext(this, true);
        context.Start();
    }
}
