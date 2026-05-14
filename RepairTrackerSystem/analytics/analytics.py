"""
REPAIR TRACKER — ANALYTICS MODULE
Generates advanced reports and graphs from SQLite database
"""

import sqlite3
import pandas as pd
import matplotlib.pyplot as plt
import matplotlib.dates as mdates
from datetime import datetime, timedelta
import os
from pathlib import Path
import json
import sys

class AnalyticsService:
    def __init__(self, db_path="repairtracker.db", output_dir=None):
        """Initialize analytics service with database connection"""
        self.db_path = db_path
        
        # Use provided output_dir or default to solution root/output/graphs
        if output_dir:
            self.output_dir = Path(output_dir)
        else:
            # Try to infer output directory relative to script location
            script_dir = Path(__file__).parent.parent  # Go up from analytics/
            self.output_dir = script_dir / "output" / "graphs"
        
        self.output_dir.mkdir(parents=True, exist_ok=True)

        # Set matplotlib style
        plt.style.use('seaborn-v0_8-darkgrid')
        self.colors = {
            'completed': '#4ade80',
            'repairing': '#60a5fa',
            'pending': '#fbbf24',
            'diagnosing': '#f87171',
            'primary': '#0ea5e9',
            'accent': '#8b5cf6'
        }

    def get_connection(self):
        """Get SQLite database connection"""
        try:
            conn = sqlite3.connect(self.db_path)
            conn.row_factory = sqlite3.Row
            return conn
        except sqlite3.Error as e:
            print(f"Database connection error: {e}")
            return None

    # ═══════════════════════════════════════════════════════════════════════
    # 1. REPAIRS PER DAY (LINE CHART)
    # ═══════════════════════════════════════════════════════════════════════
    def generate_repairs_per_day(self, days=30):
        """Generate repairs per day for last N days"""
        try:
            conn = self.get_connection()
            query = f"""
                SELECT 
                    DATE(DateReceived) as RepairDate,
                    COUNT(*) as RepairCount
                FROM Repairs
                WHERE DateReceived >= datetime('now', '-{days} days')
                GROUP BY DATE(DateReceived)
                ORDER BY RepairDate ASC
            """

            df = pd.read_sql_query(query, conn)
            conn.close()

            if df.empty:
                print("No repair data available")
                return None

            df['RepairDate'] = pd.to_datetime(df['RepairDate'])

            fig, ax = plt.subplots(figsize=(12, 6))
            ax.plot(df['RepairDate'], df['RepairCount'], 
                   marker='o', linewidth=2.5, markersize=8, 
                   color=self.colors['primary'], label='Daily Repairs')

            ax.fill_between(df['RepairDate'], df['RepairCount'], 
                           alpha=0.3, color=self.colors['primary'])

            # Add data labels on points
            for x, y in zip(df['RepairDate'], df['RepairCount']):
                ax.annotate(f'{int(y)}', (x, y), textcoords="offset points", 
                           xytext=(0,10), ha='center', fontsize=9, fontweight='bold')

            ax.set_title(f'Repairs Per Day (Last {days} Days)', fontsize=16, fontweight='bold', pad=20)
            ax.set_xlabel('Date', fontsize=12, fontweight='bold')
            ax.set_ylabel('Number of Repairs', fontsize=12, fontweight='bold')
            ax.xaxis.set_major_formatter(mdates.DateFormatter('%m/%d'))
            ax.xaxis.set_major_locator(mdates.DayLocator(interval=5))
            plt.xticks(rotation=45)
            ax.grid(True, alpha=0.3)
            ax.legend(loc='upper left', fontsize=10)

            filename = self.output_dir / f"repairs_per_day_{datetime.now().strftime('%Y%m%d_%H%M%S')}.png"
            plt.tight_layout()
            plt.savefig(filename, dpi=300, bbox_inches='tight')
            plt.close()

            print(f"✓ Repairs per day chart saved: {filename}")
            return str(filename)

        except Exception as e:
            print(f"Error generating repairs per day chart: {e}")
            return None

    # ═══════════════════════════════════════════════════════════════════════
    # 2. ISSUE FREQUENCY (BAR CHART)
    # ═══════════════════════════════════════════════════════════════════════
    def generate_issue_frequency(self, top_n=10):
        """Generate issue frequency chart"""
        try:
            conn = self.get_connection()
            query = f"""
                SELECT
                    Issue,
                    COUNT(*) as Frequency
                FROM Repairs
                WHERE Issue IS NOT NULL AND Issue != ''
                GROUP BY Issue
                ORDER BY Frequency DESC
                LIMIT {top_n}
            """

            df = pd.read_sql_query(query, conn)
            conn.close()

            if df.empty:
                print("No issue data available")
                return None

            fig, ax = plt.subplots(figsize=(12, 6))
            bars = ax.barh(df['Issue'], df['Frequency'], color=self.colors['accent'])

            # Add value labels on bars
            for i, bar in enumerate(bars):
                width = bar.get_width()
                ax.text(width, bar.get_y() + bar.get_height()/2, 
                       f'{int(width)}', ha='left', va='center', fontweight='bold', fontsize=10)

            ax.set_title(f'Top {top_n} Most Common Issues', fontsize=16, fontweight='bold', pad=20)
            ax.set_xlabel('Frequency', fontsize=12, fontweight='bold')
            ax.set_ylabel('Issue Type', fontsize=12, fontweight='bold')
            ax.grid(True, alpha=0.3, axis='x')

            filename = self.output_dir / f"issue_frequency_{datetime.now().strftime('%Y%m%d_%H%M%S')}.png"
            plt.tight_layout()
            plt.savefig(filename, dpi=300, bbox_inches='tight')
            plt.close()

            print(f"✓ Issue frequency chart saved: {filename}")
            return str(filename)

        except Exception as e:
            print(f"Error generating issue frequency chart: {e}")
            return None

    # ═══════════════════════════════════════════════════════════════════════
    # 3. TECHNICIAN PERFORMANCE (GROUPED BAR CHART)
    # ═══════════════════════════════════════════════════════════════════════
    def generate_technician_performance(self):
        """Generate technician performance report"""
        try:
            conn = self.get_connection()
            query = """
                SELECT
                    COALESCE(t.Name, 'Unassigned') as Name,
                    COUNT(r.RepairID) as TotalRepairs,
                    SUM(CASE WHEN r.Status='Completed' THEN 1 ELSE 0 END) as CompletedRepairs,
                    ROUND(AVG(CAST(r.Cost AS FLOAT)), 2) as AvgCost
                FROM Repairs r
                LEFT JOIN Technicians t ON r.TechnicianID = t.ID
                WHERE r.Cost IS NOT NULL AND r.Cost > 0
                GROUP BY r.TechnicianID
                ORDER BY CompletedRepairs DESC
            """

            df = pd.read_sql_query(query, conn)
            conn.close()

            if df.empty:
                print("No technician performance data available")
                return None

            # Replace NaN with 0
            df['AvgCost'] = df['AvgCost'].fillna(0)
            df['CompletedRepairs'] = df['CompletedRepairs'].fillna(0).astype(int)
            df['TotalRepairs'] = df['TotalRepairs'].fillna(0).astype(int)

            fig, ax = plt.subplots(figsize=(12, 6))
            x = range(len(df))
            width = 0.35

            bars1 = ax.bar([i - width/2 for i in x], df['TotalRepairs'], width, 
                          label='Total Repairs', color=self.colors['primary'], alpha=0.8)
            bars2 = ax.bar([i + width/2 for i in x], df['CompletedRepairs'], width,
                          label='Completed', color=self.colors['completed'], alpha=0.8)

            # Add value labels on bars
            for bars in [bars1, bars2]:
                for bar in bars:
                    height = bar.get_height()
                    ax.text(bar.get_x() + bar.get_width()/2., height,
                           f'{int(height)}', ha='center', va='bottom', fontsize=9, fontweight='bold')

            ax.set_xticks(x)
            ax.set_xticklabels(df['Name'], rotation=45, ha='right')
            ax.set_title('Technician Performance Metrics', fontsize=16, fontweight='bold', pad=20)
            ax.set_ylabel('Number of Repairs', fontsize=12, fontweight='bold')
            ax.legend(fontsize=10)
            ax.grid(True, alpha=0.3, axis='y')

            filename = self.output_dir / f"technician_performance_{datetime.now().strftime('%Y%m%d_%H%M%S')}.png"
            plt.tight_layout()
            plt.savefig(filename, dpi=300, bbox_inches='tight')
            plt.close()

            print(f"✓ Technician performance chart saved: {filename}")
            return str(filename)

        except Exception as e:
            print(f"Error generating technician performance chart: {e}")
            return None

    # ═══════════════════════════════════════════════════════════════════════
    # 4. COST ANALYSIS (PIE CHART)
    # ═══════════════════════════════════════════════════════════════════════
    def generate_cost_analysis(self):
        """Generate cost analysis by status"""
        try:
            conn = self.get_connection()
            query = """
                SELECT
                    Status,
                    COUNT(*) as Count,
                    ROUND(SUM(Cost), 2) as TotalCost,
                    ROUND(AVG(Cost), 2) as AvgCost
                FROM Repairs
                GROUP BY Status
                ORDER BY TotalCost DESC
            """

            df = pd.read_sql_query(query, conn)
            conn.close()

            if df.empty:
                print("No cost data available")
                return None

            fig, (ax1, ax2) = plt.subplots(1, 2, figsize=(14, 6))

            # Pie chart for cost distribution
            colors_list = [self.colors.get(status.lower(), '#999999') for status in df['Status']]
            wedges, texts, autotexts = ax1.pie(df['TotalCost'], labels=df['Status'], autopct='%1.1f%%',
                                               colors=colors_list, startangle=90, textprops={'fontsize': 10, 'fontweight': 'bold'})
            ax1.set_title('Cost Distribution by Status', fontsize=14, fontweight='bold')

            # Bar chart for average cost
            bars = ax2.bar(df['Status'], df['AvgCost'], color=colors_list, alpha=0.8)
            ax2.set_title('Average Repair Cost by Status', fontsize=14, fontweight='bold')
            ax2.set_ylabel('Average Cost (₱)', fontsize=11, fontweight='bold')
            ax2.set_xlabel('Status', fontsize=11, fontweight='bold')

            # Add value labels on bars
            for bar in bars:
                height = bar.get_height()
                ax2.text(bar.get_x() + bar.get_width()/2., height,
                        f'${height:.2f}', ha='center', va='bottom', fontsize=9, fontweight='bold')

            ax2.grid(True, alpha=0.3, axis='y')
            plt.setp(ax2.xaxis.get_majorticklabels(), rotation=45, ha='right')

            filename = self.output_dir / f"cost_analysis_{datetime.now().strftime('%Y%m%d_%H%M%S')}.png"
            plt.tight_layout()
            plt.savefig(filename, dpi=300, bbox_inches='tight')
            plt.close()

            print(f"✓ Cost analysis chart saved: {filename}")
            return str(filename)

        except Exception as e:
            print(f"Error generating cost analysis chart: {e}")
            return None

    # ═══════════════════════════════════════════════════════════════════════
    # 5. TREND ANALYSIS (STACKED AREA CHART)
    # ═══════════════════════════════════════════════════════════════════════
    def generate_trend_analysis(self):
        """Generate weekly trend analysis"""
        try:
            conn = self.get_connection()
            query = """
                SELECT
                    strftime('%Y-W%W', DateReceived) as Week,
                    Status,
                    COUNT(*) as Count
                FROM Repairs
                WHERE DateReceived >= datetime('now', '-12 weeks')
                GROUP BY Week, Status
                ORDER BY Week ASC
            """

            df = pd.read_sql_query(query, conn)
            conn.close()

            if df.empty:
                print("No trend data available")
                return None

            # Pivot data for stacked chart
            pivot_df = df.pivot_table(index='Week', columns='Status', values='Count', fill_value=0)

            fig, ax = plt.subplots(figsize=(14, 6))
            pivot_df.plot(kind='area', stacked=True, ax=ax, alpha=0.7,
                         color=[self.colors.get(status.lower(), '#999999') for status in pivot_df.columns])

            ax.set_title('Repair Trends - Last 12 Weeks', fontsize=16, fontweight='bold', pad=20)
            ax.set_xlabel('Week', fontsize=12, fontweight='bold')
            ax.set_ylabel('Number of Repairs', fontsize=12, fontweight='bold')
            ax.legend(title='Status', loc='upper left', fontsize=10)
            ax.grid(True, alpha=0.3)
            plt.setp(ax.xaxis.get_majorticklabels(), rotation=45, ha='right')

            filename = self.output_dir / f"trend_analysis_{datetime.now().strftime('%Y%m%d_%H%M%S')}.png"
            plt.tight_layout()
            plt.savefig(filename, dpi=300, bbox_inches='tight')
            plt.close()

            print(f"✓ Trend analysis chart saved: {filename}")
            return str(filename)

        except Exception as e:
            print(f"Error generating trend analysis chart: {e}")
            return None

    # ═══════════════════════════════════════════════════════════════════════
    # GENERATE ALL REPORTS
    # ═══════════════════════════════════════════════════════════════════════
    def generate_all_reports(self):
        """Generate all available reports"""
        print("\n" + "="*60)
        print("REPAIR TRACKER — ANALYTICS REPORT GENERATION")
        print("="*60 + "\n")
        
        self.generate_repairs_per_day(days=30)
        self.generate_issue_frequency(top_n=10)
        self.generate_technician_performance()
        self.generate_cost_analysis()
        self.generate_trend_analysis()
        
        print("\n" + "="*60)
        print(f"All reports generated in: {self.output_dir}")
        print("="*60 + "\n")


if __name__ == "__main__":
    analytics = AnalyticsService()
    analytics.generate_all_reports()