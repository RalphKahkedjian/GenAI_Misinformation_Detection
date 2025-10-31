namespace GenAiMisinformation
{
    public class MisinformationRecord
    {
        public int id { get; set; }
        public string post_id { get; set; }
        public string platform { get; set; }
        public string timestamp { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string month { get; set; }
        public string weekday { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string timezone { get; set; }
        public string author_id { get; set; }
        public string author_followers { get; set; }
        public string author_verified { get; set; }
        public string text { get; set; }
        public int text_length { get; set; }
        public int token_count { get; set; }
        public double readability_score { get; set; }
        public int num_urls { get; set; }
        public int num_mentions { get; set; }
        public int num_hashtags { get; set; }
        public double sentiment_score { get; set; }
        public double toxicity_score { get; set; }
        public string model_signature { get; set; }
        public double detected_synthetic_score { get; set; }
        public double embedding_sim_to_facts { get; set; }
        public string factcheck_verdict { get; set; }
        public int external_factchecks_count { get; set; }
        public double source_domain_reliability { get; set; }
        public double engagement { get; set; }
        public bool is_misinformation { get; set; }
    }
}
