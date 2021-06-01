namespace Deblue.DialogSystem
{
    public readonly struct Dialog_Choice_Maded
    {
        public readonly Choice Choice;

        public Dialog_Choice_Maded(Choice choice)
        {
            Choice = choice;
        }
    }

    public readonly struct Dialog_Give_Choice
    {
        public readonly Choice[] Choices;
        public readonly string   ChoiceTextId;

        public Dialog_Give_Choice(Choice[] choices, string choiceTextId)
        {
            Choices = choices;
            ChoiceTextId = choiceTextId;
        }
    }

    public readonly struct Dialog_Start
    {
        public readonly DialogSO  Dialog;
        public readonly Character Character;

        public Dialog_Start(DialogSO dialog, Character character)
        {
            Dialog = dialog;
            Character = character;
        }
    }
    
    public readonly struct Dialog_End
    {
        public readonly DialogSO  Dialog;
        public readonly Character Character;

        public Dialog_End(DialogSO dialog, Character character)
        {
            Dialog = dialog;
            Character = character;
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
}
