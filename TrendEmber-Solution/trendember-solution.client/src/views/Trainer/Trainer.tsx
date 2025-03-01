import React, { useEffect, useRef, useState } from 'react';
import { createChart, CandlestickSeries, UTCTimestamp, ISeriesApi, SeriesType, IChartApi  } from 'lightweight-charts';
import { useGetTrainerSample } from '@api/endpoints/trainer/hooks';

interface CandlestickData {
    time: UTCTimestamp,
    open: number,
    high: number,
    low: number,
    close: number    
}

const Trainer = () => {
  const chartContainerRef = useRef<HTMLDivElement>(null);
  const [isButtonClicked, setIsButtonClicked] = useState(false);
  const { data, isLoading, isError, refetch } = useGetTrainerSample(isButtonClicked);
  const [candleStickSeries, setCandleStickSeries] = useState<ISeriesApi<SeriesType,UTCTimestamp>>();
  const [isDoji,setDojiFlag] = useState(false);
  const [isHammer,setHammerFlag] = useState(false);
  const [isFullBar,setFullBarFlag] = useState(false);
  
  useEffect(() => {
    if (!chartContainerRef.current) return;

    // Create a new chart
    const chart = createChart(chartContainerRef.current, {
      width: 600,
      height: 400,
    });

    const chartSeries =chart.addSeries(
        CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350', borderVisible: false, wickUpColor: '#26a69a', wickDownColor: '#ef5350' });
    setCandleStickSeries(chartSeries);
    const candleStickData: CandlestickData[] = [
        { time: new Date('2024-02-20T00:00:00Z').getTime() as UTCTimestamp, open: 10, high: 10.63, low: 9.49, close: 9.55 },
      ];

      chartSeries.setData(candleStickData);
    
    chart.timeScale().fitContent();

    // Cleanup chart on component unmount
    return () => {
      chart.remove();
    };

  }, []);

function calculateIsDoji(open: number, close: number, high: number, low: number): boolean {
  const range = (high - low) / 3;
  const threshold = low + range;
  return open >= threshold && close >= threshold && Math.abs(open - close) <= range;
}

function calculateIsHammer(open: number, close: number, high: number, low: number): boolean {
  const range = (high - low) / 8;
  const threshold = high - (high - low) / 3;
  const maxOC = Math.max(open, close);
  const minOC = Math.min(open, close);

  return maxOC >= threshold &&
         minOC >= threshold &&
         (minOC <= threshold + range && maxOC >= high - range);
}

function calculateIsFullBar(open: number, close: number, high: number, low: number): boolean {
  const range = (high - low) / 6;
  return Math.max(open, close) >= high - range &&
         Math.min(open, close) <= low + range;
}



  useEffect(() => {
    if (data) {

    const candleStickData: CandlestickData[] = [
        { time: new Date('2024-02-20T00:00:00Z').getTime() as UTCTimestamp, open: data.data.open, high: data.data.high, low: data.data.low, close: data.data.close},
      ];
      if (candleStickSeries)
      {
        candleStickSeries.setData(candleStickData);
        setDojiFlag(calculateIsDoji(data.data.open,data.data.close,data.data.high,data.data.low));
        setHammerFlag(calculateIsHammer(data.data.open,data.data.close,data.data.high,data.data.low));
        setFullBarFlag(calculateIsFullBar(data.data.open,data.data.close,data.data.high,data.data.low));
      }
    }
  }, [data]);

  const handleButtonClick = () => {
    setIsButtonClicked(true);
    refetch();
  }


  return (<div>
  <button onClick={handleButtonClick}>New Candle</button>
  {isDoji && <p>IT IS A DOJI</p>}
  {isHammer && <p>IT IS A HAMMER</p>}
  {isFullBar && <p>IT IS A FULL BAR</p>}
  <div ref={chartContainerRef} style={{ width: '100%', height: '400px' }} />
  </div>);
};

export default Trainer;
