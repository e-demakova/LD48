namespace Deblue.DialogSystem
{
    public readonly struct Dialog_Start
    {
        public readonly DialogSO Dialog;

        public Dialog_Start(DialogSO dialog)
        {
            Dialog = dialog;
        }
    }
    
    public readonly struct Dialog_End
    {
        public readonly DialogSO Dialog;

        public Dialog_End(DialogSO dialog)
        {
            Dialog = dialog;
        }
    }
    
    public readonly struct Replica_Switch
    {
        public readonly Replica Replica;

        public Replica_Switch(Replica replica)
        {
            Replica = replica;
        }
    }

    public readonly struct Dialogues_Over
    {
    }
}
