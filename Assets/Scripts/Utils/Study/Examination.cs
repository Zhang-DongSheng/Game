using System;
using System.Collections.Generic;

namespace Game.Study
{
    public class Examination
    {
        private readonly Dictionary<string, Func<Question, Answer>> papers = new Dictionary<string, Func<Question, Answer>>();

        public void Register(string key, Func<Question, Answer> func)
        {
            if (papers.ContainsKey(key))
            {
                if (papers[key] != null)
                {
                    Delegate[] dels = papers[key].GetInvocationList();
                    foreach (Delegate del in dels)
                    {
                        if (del.Equals(func))
                            return;
                    }
                }
                papers[key] += func;
            }
            else
            {
                papers[key] = func;
            }
        }

        public void Unregister(string key, Func<Question, Answer> func)
        {
            if (papers.ContainsKey(key))
            {
                papers[key] -= func;
            }
        }

        public Answer Invoke(string key, Question question)
        {
            if (papers.TryGetValue(key, out Func<Question, Answer> func))
            {
                return func?.Invoke(question);
            }
            return null;
        }
    }

    public class Question
    {
        public string content;

        public List<string> options;
    }

    public class Answer
    {
        public string result;
    }
}