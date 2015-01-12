using Shiretoko.Model;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System;

namespace Shiretoko
{
    public class User
    {
        public bool IsPlayer { get; set; }

        public string Name { get; set; }

        public bool IsPositionInJctArea { get; set; } = false;

        public int TurnIndex { get; set; }

        public KomaColor Color { get; set; }
        public int Index { get; internal set; } = 0;

        public UIElement Piece { get; set; }

        public int BreakTurns { get; set; }

        public void ComputeFunction(string functionName)
        {
            functionName = functionName.Replace("Bonus", "");
            if (functionName.Contains("Advance"))
            {
                var admasu = int.Parse(functionName.Replace("Advance", ""));
                CheckThroughMasume(admasu, masumes,true);
            }
            else if (functionName.Contains("Back"))
            {
                int bamasu = 0;
                if (functionName.Contains("RandNum"))
                {
                    var i = new Random().Next(6) + 1;
                    MessageBox.Show("\{i}マスもどる！");
                    bamasu = i;
                }
                else
                    bamasu = int.Parse(functionName.Replace("Back", ""));
                bamasu = 0 - bamasu;
                CheckThroughMasume(bamasu, masumes,true);
            }
            else if (functionName.Contains("Break"))
            {
                this.BreakTurns += int.Parse(functionName.Replace("Break", ""));
            }
            else if (functionName.Contains("Jump"))
            {
                int i = 0;
                if (!functionName.Contains("Start"))
                    i = int.Parse(functionName.Replace("Jump",""));
                Index = i;
            }
            else if (functionName == "Junction")
            {
                var r = IsPositionInJctArea = Convert.ToBoolean(new Random().Next(2));
                var str = r ? "あんた下" : "あんた右";
                MessageBox.Show(str);
                
            }
        }

        public void CheckThroughMasume(int advanceOrBackMasume, List<Masume> masumeList, bool isFunctionThrough)
        {
            masumes = masumeList;
            if (advanceOrBackMasume > 0)
                Index++;
            else
                Index--;
            Masume nowMasume = null;


            for (int i = 0; i < advanceOrBackMasume; i++)
            {

                if (this.IsPositionInJctArea)
                {
                    nowMasume = masumeList.Where(x => x.JctIndex == Index).First();
                }
                else
                {
                    try
                    {
                        nowMasume = masumeList.Where(x => x.Index == Index).First();
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Goalおめでとう！");
                    }
                }
                if (nowMasume.FunctionName.Contains("All") || nowMasume.FunctionName == "Junction")
                    ComputeFunction(nowMasume.FunctionName.Replace("All",""));
                if (advanceOrBackMasume > 0)
                    Index++;
                else
                    Index--;

            }

            if (advanceOrBackMasume > 0)
                Index--;
            else
                Index++;
            if (!isFunctionThrough)
                ComputeFunction(nowMasume.FunctionName);

        }

        private List<Masume> masumes;

    }
}