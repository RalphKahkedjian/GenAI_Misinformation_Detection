namespace GenAiMisinformation
{
    namespace GenAiMisinformation
    {
        public class MisinformationRecord
        {
            public int id { get; set; }

            public string post_id { get; set; } = string.Empty;
            public string platform { get; set; } = string.Empty;
            public string month { get; set; } = string.Empty;
            public string country { get; set; } = string.Empty;
            public string author_id { get; set; } = string.Empty;
            public string author_followers { get; set; } = string.Empty;
            public string author_verified { get; set; } = string.Empty;
            public string text { get; set; } = string.Empty;

            public int text_length { get; set; }
            public int token_count { get; set; }
            public double readability_score { get; set; }
            public int num_mentions { get; set; }
            public int num_hashtags { get; set; }
            public double sentiment_score { get; set; }
            public double toxicity_score { get; set; }

            public string model_signature { get; set; } = string.Empty;
            public double detected_synthetic_score { get; set; }
            public string factcheck_verdict { get; set; } = string.Empty;
            public int external_factchecks_count { get; set; }
            public double source_domain_reliability { get; set; }
            public double engagement { get; set; }

            public bool is_misinformation { get; set; }
        }
    }
}
