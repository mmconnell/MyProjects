/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package snakeplaytest;

import static snakeplaytest.SnakeMap.transition;

/**
 *
 * @author Michael
 */
public enum Direction {
    UP, LEFT, RIGHT, DOWN;

        public static int moveX(Direction dir)
        {
            int total = 0;
            switch(dir)
            {
                case LEFT:
                    total -= transition;
                    break;
                case RIGHT:
                    total += transition;
                    break;
            }
            return total;
        }

        public static int moveY(Direction dir)
        {
            int total = 0;
            switch(dir)
            {
                case DOWN:
                    total += transition;
                    break;
                case UP:
                    total -= transition;
                    break;
            }
            return total;
        }
}
