// Version: Build 5
//
// Build 5 - Saturday, November 30th, 2013
//   * (10:36) Ported to C#.
//
// Build 4 - Sunday, August 12th, 2012
//   * (19:31) Update: To support xna, since xna does not support the color type, replaced the
//     variable with the seperate integer variables for R, G, and B.
//
// Build 3 - Friday, March 27th, 2009
//   * (14:02) Update: in the update functions, declared locally defined
//     variables to class-level to get a little speed increase.
//
// Build 2 - Tuesday, December 23rd, 2008
//   * (19:02) Update: Added support for a base renderer.
//
// Build 1
//   * Created.

namespace HelperLib
{
    public class FrameTools
    {
        private const float MaxDeltaTime            = 0.05F;
        private const float MilisecondsPerSecondDiv = 0.001F;

        private int mLastTick = 0;
        private int mLastFrameRate = 0;
        private int mFrameRate = 0;               //Used by CalcFrameRate() to determine how many frames are rendered per second.
        private int mDelta_LastTick = 0;          //The last tick count used by CalcDelta() to determine the difference in time between frames.
        private int mMaxKnownFrameRate = 0;       //Used by DrawFPS() to store the largest FPS encountered.
        private int mTempCurrentTime = 0;         //Used by CalcFrameRate() and CalcDelta() to reduce variable declaration in update.
        private int mTempCurrentFPS = 0;          //Used by DrawFPS() to reduce variable declaration in update.
        private int mTempLastDiff = 0;
        private float mTempDeltaTime = 0;         //Used by CalcDelta() to reduce variable declaration in update.
        private BaseCanvas mCanvas = null;        //The text renderer to use to draw the FPS to the screen.

        public int CalculateFrameRate()
        {
            this.mTempCurrentTime = System.Environment.TickCount;
            if((this.mTempCurrentTime - this.mLastTick) >= 1000) {
                this.mLastFrameRate = this.mFrameRate;
                this.mFrameRate = 0;
                this.mLastTick = this.mTempCurrentTime;
            }
            this.mFrameRate += 1;

            return this.mLastFrameRate;
        } //CalculateFrameRate function

        public float CalcDelta()
        {
            // Get the current this.
            this.mTempCurrentTime = System.Environment.TickCount;
            if(this.mDelta_LastTick == 0) this.mDelta_LastTick = this.mTempCurrentTime;
            this.mTempLastDiff = (this.mTempCurrentTime - this.mDelta_LastTick);

            // Store the last delta time tick.
            this.mDelta_LastTick = this.mTempCurrentTime;

            // Calculate the Global Time Delta value.
            this.mTempDeltaTime = (this.mTempLastDiff * MilisecondsPerSecondDiv);
            // Prevent the delta time from getting too big, this is just in case the application's instance was stalled ...
            if(this.mTempDeltaTime > MaxDeltaTime) this.mTempDeltaTime = MaxDeltaTime;

            return this.mTempDeltaTime;
        }  //CalcDelta

        public void SetCanvas(BaseCanvas canvas) { this.mCanvas = canvas; }

        public void DrawFps(int textColorR, int textColorG, int textColorB, bool showMaxFps = true)
        {
            if(this.mCanvas == null) return;

            this.mTempCurrentFPS = this.CalculateFrameRate();
            if(this.mTempCurrentFPS > this.mMaxKnownFrameRate) this.mMaxKnownFrameRate = this.mTempCurrentFPS;

            this.mCanvas.DrawFps(this.mTempCurrentFPS, this.mMaxKnownFrameRate, textColorR, textColorG, textColorB, showMaxFps);
        } //DrawFPS

        public interface BaseCanvas
        {
            void DrawFps(int fps, int fpsMax, int textColorR, int textColorG, int textColorB, bool showMaxFps);
        } //BaseCanvas
    } //FrameTools
} // HelperLib namespace
