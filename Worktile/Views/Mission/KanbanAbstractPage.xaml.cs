﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Worktile.Common;
using Worktile.Models.Kanban;

namespace Worktile.Views.Mission
{
    public abstract partial class KanbanAbstractPage : Page
    {
        public KanbanAbstractPage()
        {
            InitializeComponent();
        }

        protected bool IsPageLoaded { get; set; }
        protected abstract Grid MyGrid { get; }

        protected void ReadForProgressBar(KanbanGroup kbGroup, int state)
        {
            switch (state)
            {
                case 1: kbGroup.NotStarted++; break;
                case 2: kbGroup.Processing++; break;
                case 3: kbGroup.Completed++; break;
            }
        }

        protected KanbanItemProperty GetDueProperty(DateTime? dueDate)
        {
            if (dueDate.HasValue)
            {
                return new KanbanItemProperty
                {
                    Name = "截止时间",
                    Value = dueDate.Value.ToWtKanbanDate(),
                    Foreground = WtColorHelper.GetForegroundWithExpire(dueDate.Value),
                    Background = WtColorHelper.GetBackgroundWithExpire(dueDate.Value)
                };
            }
            return null;
        }

        protected async void MyGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var sp = MyGrid.GetChild<StackPanel>("MissionHeader");
            var lvs = MyGrid.GetChildren<ListView>("DataList");
            if (sp != null && lvs.Any())
            {
                foreach (var item in lvs)
                {
                    item.MaxHeight = MyGrid.ActualHeight - 24 - sp.ActualHeight;
                }
            }
            else
            {
                for (int i = 0; i < 200; i++)
                {
                    if (IsPageLoaded)
                    {
                        sp = MyGrid.GetChild<StackPanel>("MissionHeader");
                        lvs = MyGrid.GetChildren<ListView>("DataList");
                        if (sp != null && lvs.Any())
                        {
                            foreach (var item in lvs)
                            {
                                item.MaxHeight = MyGrid.ActualHeight - 24 - sp.ActualHeight;
                            }
                            return;
                        }
                    }
                    await Task.Delay(100);
                }
            }
        }
    }
}