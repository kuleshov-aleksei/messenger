using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Messenger.ChatFiller
{
    internal class TranscriptReader
    {
        private string m_script;
        private string m_name;
        private List<string> m_actors = new List<string>();
        private List<Line> m_replics = new List<Line>();
        private Regex m_actorRegex = new Regex(@"(?'actor'[\w\d \.]+)(?'remove' \(.+\))*", RegexOptions.Compiled);

        public TranscriptReader(string script)
        {
            this.m_script = script;
        }

        internal void Read()
        {
            string[] rawLines = File.ReadAllLines(m_script);
            string previousActor = string.Empty;
            foreach (string rawLine in rawLines)
            {
                if (rawLine.StartsWith("#! Name ="))
                {
                    m_name = rawLine.Split('=')[1].Substring(1);
                    continue;
                }

                if (rawLine.StartsWith('#'))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    continue;
                }

                if (char.IsUpper(rawLine[0]) && rawLine.Contains(':'))
                {
                    int delimiterPosition = rawLine.IndexOf(':');
                    ReadOnlySpan<char> rawSpan = rawLine;
                    ReadOnlySpan<char> actorSpan = rawSpan.Slice(0, delimiterPosition);
                    ReadOnlySpan<char> lineSpan = rawSpan.Slice(delimiterPosition + 1, rawSpan.Length - delimiterPosition - 1);
                
                    string actor = actorSpan.ToString();
                    Match match = m_actorRegex.Match(actor);
                    actor = match.Groups["actor"].Value;

                    string replica = lineSpan.ToString().Trim();

                    if (!m_actors.Contains(actor))
                    {
                        m_actors.Add(actor);
                    }

                    m_replics.Add(new Line(actor, replica));

                    previousActor = actor;
                }
                else if (previousActor != string.Empty)
                {
                    string replica = rawLine;
                    m_replics.Add(new Line(previousActor, replica));
                }
            }
        }

        internal string GetName()
        {
            return m_name;
        }

        internal List<string> GetActors()
        {
            return m_actors;
        }

        internal List<Line> GetLines()
        {
            return m_replics;
        }

        public record Line (string Actor, string Replica);
    }
}