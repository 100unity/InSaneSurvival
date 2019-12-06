namespace Constants
{
    public static class Consts
    {
        public static class Scene
        {
            public const int MAIN_MENU = 0;
            public const int GAME = 1;
        }
        
        public static class Enemy
        {
            // maxDistance for mapping random points onto the NavMesh
            public const float MAX_NAVMESH_MAPPING_DISTANCE = 1.0f;

            // just a high number to ensure random point generation for wandering
            public const int MAPPING_ITERATIONS = 500; 
        }
    }
}

