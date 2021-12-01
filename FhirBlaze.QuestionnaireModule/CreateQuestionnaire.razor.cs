﻿using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hl7.Fhir.Model.Questionnaire;

namespace FhirBlaze.QuestionnaireModule
{
    [Authorize]
    public partial class CreateQuestionnaire
    {
        [Inject]
        IFhirService FhirService { get; set; }
        public Questionnaire Questionnaire { get; set; } = new Questionnaire();

       

        protected override System.Threading.Tasks.Task OnInitializedAsync()
        {
            InitializeQuestionnaire();
            return base.OnInitializedAsync();
        }
        protected async void InitializeQuestionnaire()
        {
            var QuestionnaireIdentifier = new Hl7.Fhir.Model.Identifier();
            QuestionnaireIdentifier.System = "http://hlsemops.microsoft.com";
            QuestionnaireIdentifier.Value = Guid.NewGuid().ToString();
            Questionnaire.Identifier = new List<Hl7.Fhir.Model.Identifier>();
            Questionnaire.Identifier.Add(QuestionnaireIdentifier);
            Questionnaire.Status = PublicationStatus.Draft;
            Questionnaire.Title = "New Questionnaire";
            Questionnaire.Description.Value = "A new questionnaire";
            
        }
        protected void AddItem(QuestionnaireItemType type)
        {
            var nItem = new Questionnaire.ItemComponent();
            nItem.Type = type;
            Questionnaire.Item.Add(nItem);
        }
        protected void RemoveItem(Questionnaire.ItemComponent Item)
        {
            Questionnaire.Item.Remove(Item);
        }

        protected string GetHeader(Questionnaire.ItemComponent item)
        {
            return (item.Type.Equals(Questionnaire.QuestionnaireItemType.Group))?"Group": $"{item.Type.ToString()} Question";        
        }
        public async void Submit()
        {
            Questionnaire.Status = PublicationStatus.Draft;
            await FhirService.CreateQuestionnaireAsync(Questionnaire);
        }
    }
}
