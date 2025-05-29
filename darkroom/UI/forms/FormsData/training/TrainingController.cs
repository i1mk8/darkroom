using darkroom.UI.forms.FormsData.game;
using darkroom.UI.forms.FormsData.KeyEvent;
using darkroom.UI.resources;
using darkroom.UI.sound;

namespace darkroom.UI.forms.FormsData.training;

public class TrainingController(TrainingFormData formData) : GameController(formData)
{
    private readonly Sound _selectSound = new(Resources.SelectSoundPath);
    
    protected override void InitializeKeyEvent(out KeyEvent.KeyEvent keyEvent)
    {
        KeyEventsActions.Add(new KeyEventAction(Keys.Enter, true, OnContinue));
        base.InitializeKeyEvent(out keyEvent);
    }

    private void OnContinue()
    {
        if (formData.HintIndex >= formData.Hints.Count - 1)
            return;
            
        _selectSound.PlaySound(1);
        formData.HintIndex++;
    }
}