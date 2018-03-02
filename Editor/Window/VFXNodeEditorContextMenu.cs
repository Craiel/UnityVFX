namespace Assets.Scripts.Craiel.VFX.Editor.Window
{
    using System;
    using Essentials.Editor;
    using Essentials.Event;
    using Essentials.Event.Editor;
    using Events;

    public class VFXNodeEditorContextMenu : DynamicContextMenu
    {
        private BaseEventSubscriptionTicket eventVfxComponentsChangedTicket;
        
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public VFXNodeEditorContextMenu()
        {
            this.eventVfxComponentsChangedTicket = EditorEvents.Subscribe<EditorEventVFXComponentsChanged>(this.OnVFXComponentsChanged);

            this.RebuildMenu();
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                this.eventVfxComponentsChangedTicket.Dispose();
                this.eventVfxComponentsChangedTicket = null;
            }
            
            base.Dispose(isDisposing);
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void OnCreateComponent()
        {
            
        }
        
        private void OnVFXComponentsChanged(EditorEventVFXComponentsChanged eventdata)
        {
            this.RebuildMenu();
        }

        private void RebuildMenu()
        {
            this.Clear();
            foreach (Type type in VFXEditorCore.Components)
            {
                this.RegisterAction(type.Name, this.OnCreateComponent);
            }
        }
    }
}