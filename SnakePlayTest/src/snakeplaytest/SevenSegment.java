/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package snakeplaytest;

import javafx.beans.binding.DoubleBinding;
import javafx.beans.property.DoubleProperty;
import javafx.beans.property.ReadOnlyDoubleProperty;
import javafx.geometry.HPos;
import javafx.geometry.VPos;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.layout.Region;
import javafx.scene.paint.Color;


/**
 *
 * @author Michael
 */
public class SevenSegment extends Region
{
    private Canvas mCanvas;
    private int mCurrentNum;
    private boolean[][] onOff;
    
    private static final double widthX = 24;
    private static final double heightY = 36;
    private static final double shift = 12;
    private static final double aspect = heightY/widthX;
    private static final double startX = 2;
    private static final double startY = 2;
    private static final double[] xVal = {0, 2, 10, 12, 10, 2, 0};
    private static final double[] yVal = {0, -1, -1, 0, 1, 1, 0};
    private HPos wAlign;
    private VPos hAlign;
    
    public SevenSegment(DoubleBinding heightProperty, DoubleBinding widthProperty, HPos wAlign, VPos hAlign)
    {
        super();
        this.mCanvas = new Canvas();
        this.getChildren().add(mCanvas);
        this.mCurrentNum = 10;
        this.onOff = new boolean[5][3];
        this.prefHeightProperty().bind(heightProperty);
        this.prefWidthProperty().bind(widthProperty);
        this.wAlign = wAlign;
        this.hAlign = hAlign;
        setOnOff();
    }
    
    public SevenSegment(int val, DoubleBinding heightProperty, DoubleBinding widthProperty, HPos wAlign, VPos hAlign)
    {
        this(heightProperty, widthProperty, wAlign, hAlign);
        if(val < 0 || val > 10)
        {
            val = 10;
        }
        this.mCurrentNum = val;
        setOnOff();
    }
    
    private void setOnOff()
    {
        for (boolean[] onOff1 : onOff) 
        {
            for (int y = 0; y < onOff1.length; y++) 
            {
                onOff1[y] = false;
            }
        }
        onOff[0][1] = mCurrentNum == 2 || mCurrentNum == 3
                || mCurrentNum == 5 || mCurrentNum == 6
                || mCurrentNum == 7 || mCurrentNum == 8
                || mCurrentNum == 9 || mCurrentNum == 0;
        
        onOff[1][0] = mCurrentNum == 4 || mCurrentNum == 5
                || mCurrentNum == 6 || mCurrentNum == 8
                || mCurrentNum == 9 || mCurrentNum == 0;
        
        onOff[1][2] = mCurrentNum == 1 || mCurrentNum == 2
                || mCurrentNum == 3 || mCurrentNum == 4
                || mCurrentNum == 7 || mCurrentNum == 8
                || mCurrentNum == 9 || mCurrentNum == 0;
        
        onOff[2][1] = mCurrentNum == 2 || mCurrentNum == 3
                || mCurrentNum == 4 || mCurrentNum == 5
                || mCurrentNum == 6 || mCurrentNum == 8
                || mCurrentNum == 9;
        
        onOff[3][0] = mCurrentNum == 2 || mCurrentNum == 6
                || mCurrentNum == 8 || mCurrentNum == 0;
        
        onOff[3][2] = mCurrentNum == 1 || mCurrentNum == 3
                || mCurrentNum == 4 || mCurrentNum == 5
                || mCurrentNum == 6 || mCurrentNum == 7
                || mCurrentNum == 8 || mCurrentNum == 9
                || mCurrentNum == 0;
        
        onOff[4][1] = mCurrentNum == 2 || mCurrentNum == 3
                || mCurrentNum == 5 || mCurrentNum == 6
                || mCurrentNum == 8 || mCurrentNum == 9
                || mCurrentNum == 0;
    }
    
    @Override
    public void layoutChildren()
    {
        super.layoutChildren();
        
        double xValue = getWidth();
        double yValue = getHeight();
       
        double possibleW = yValue/(aspect);
        double possibleH = xValue*(aspect);
        
        double finalW = 0;
        double finalH = 0;
        
        if(possibleH > yValue)
        {
            finalH = yValue;
            finalW = possibleW;
        }
        else
        {
            finalH = possibleH;
            finalW = xValue;
        }
        
        mCanvas.setWidth(finalW);
        mCanvas.setHeight(finalH);
        this.layoutInArea(mCanvas, 0, 0, xValue, yValue, 0, wAlign, hAlign);
        draw();
    }
    
    public void draw()
    {
        GraphicsContext g = mCanvas.getGraphicsContext2D();
        g.clearRect(0, 0, mCanvas.getWidth(), mCanvas.getHeight());
        
        g.save();
       
        g.scale(mCanvas.getWidth()/widthX, mCanvas.getHeight()/heightY);
        g.translate(startX, startY);
        
        setColor(onOff[0][1], g);
        g.fillPolygon(xVal, yVal, xVal.length);
        
        g.save();
        g.translate(shift, 0);
        g.rotate(90);
        setColor(onOff[1][2], g);
        g.fillPolygon(xVal, yVal, xVal.length);
        
        g.translate(shift, 0);
        setColor(onOff[3][2], g);
        g.fillPolygon(xVal, yVal, xVal.length);
        
        g.restore();
        g.save();
        g.rotate(90);
        setColor(onOff[1][0], g);
        g.fillPolygon(xVal, yVal, xVal.length);
        
        g.translate(shift, 0);
        setColor(onOff[3][0], g);
        g.fillPolygon(xVal, yVal, xVal.length);
        
        g.restore();
        g.translate(0, shift);
        setColor(onOff[2][1], g);
        g.fillPolygon(xVal, yVal, xVal.length);
        
        g.translate(0, shift);
        setColor(onOff[4][1], g);
        g.fillPolygon(xVal, yVal, xVal.length);
        
        g.restore();
        
    }
    
    public void setNum(int num)
    {
        if(num >= 0 && num <= 10)
        {
            mCurrentNum = num;
            setOnOff();
        }
    }
    
    public int getNum()
    {
        return mCurrentNum;
    }
    
    private void setColor(boolean isOn, GraphicsContext gc)
    {
        if(isOn)
        {
            gc.setGlobalAlpha(1);
            gc.setFill(Color.RED);
        }
        else
        {
            gc.setGlobalAlpha(.3);
            gc.setFill(Color.DARKRED);
        }
    }
}
