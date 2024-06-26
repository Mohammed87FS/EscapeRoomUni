using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace libs
{
    public class DialogContainer
    {
        public List<Dialogue> Dialogs { get; set; }
    }

    public class DialogueManager
    {
        private Dictionary<int, Dialogue> dialogues;

        public DialogueManager(string jsonFilePath)
        {
            var json = File.ReadAllText(jsonFilePath);
            var dialogContainer = JsonConvert.DeserializeObject<DialogContainer>(json);
            dialogues = dialogContainer.Dialogs.ToDictionary(d => d.Id);
        }

        public Dialogue GetDialogueById(int id)
        {
            dialogues.TryGetValue(id, out var dialogue);
            return dialogue;
        }
    }
}
