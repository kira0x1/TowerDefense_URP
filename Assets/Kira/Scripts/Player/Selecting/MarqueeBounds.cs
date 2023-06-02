namespace Kira
{
    public struct MarqueeBounds
    {
        public float x;
        public float y;
        public float x1;
        public float y1;

        public MarqueeBounds(float x, float y, float x1, float y1)
        {
            this.x = x;
            this.y = y;
            this.x1 = x1;
            this.y1 = y1;
        }

        public void Set(float x, float y, float x1, float y1)
        {
            this.x = x;
            this.y = y;
            this.x1 = x1;
            this.y1 = y1;
        }

        public void SetStart(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void SetEnd(float x1, float y1)
        {
            this.x1 = x1;
            this.y1 = y1;
        }
    }
}