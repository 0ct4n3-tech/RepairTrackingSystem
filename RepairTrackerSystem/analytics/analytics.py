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
                   color=self.colors['primary'])

            ax.fill_between(df['RepairDate'], df['RepairCount'], 
                           alpha=0.3, color=self.colors['primary'])

            ax.set_title('Repairs Per Day (Last 30 Days)', fontsize=16, fontweight='bold', pad=20)
            ax.set_xlabel('Date', fontsize=12, fontweight='bold')
            ax.set_ylabel('Number of Repairs', fontsize=12, fontweight='bold')
            ax.xaxis.set_major_formatter(mdates.DateFormatter('%m/%d'))
            ax.xaxis.set_major_locator(mdates.DayLocator(interval=5))
            plt.xticks(rotation=45)
            ax.grid(True, alpha=0.3)

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
        """Generate bar chart of most common issues"""
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
            bars = ax.barh(range(len(df)), df['Frequency'], color=self.colors['accent'])

            ax.set_yticks(range(len(df)))
            ax.set_yticklabels(df['Issue'], fontsize=10)
            ax.set_xlabel('Frequency', fontsize=12, fontweight='bold')
            ax.set_title(f'Top {top_n} Most Common Issues', fontsize=16, fontweight='bold', pad=20)
            ax.invert_yaxis()

            # Add value labels on bars
            for i, bar in enumerate(bars):
                width = bar.get_width()
                ax.text(width, bar.get_y() + bar.get_height()/2, 
                       f'{int(width)}', ha='left', va='center', fontweight='bold')

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
    # 3. TECHNICIAN PERFORMANCE (PIE CHART + TOP PERFORMERS)
    # ═══════════════════════════════════════════════════════════════════════
    def generate_technician_performance(self):
        """Generate technician performance metrics"""
        try:
            conn = self.get_connection()

            # Repairs completed per technician
            query = """
                SELECT 
                    COALESCE(t.Name, 'Unassigned') as Name,
                    COUNT(r.RepairID) as TotalRepairs,
                    SUM(CASE WHEN r.Status='Completed' THEN 1 ELSE 0 END) as CompletedRepairs,
                    ROUND(AVG(r.Cost), 2) as AvgCost
                FROM Repairs r
                LEFT JOIN Technicians t ON r.TechnicianID = t.ID
                GROUP BY r.TechnicianID
                ORDER BY CompletedRepairs DESC
            """

            df = pd.read_sql_query(query, conn)
            conn.close()

            if df.empty:
                print("No technician data available")
                return None

            # Pie chart - Repairs by technician
            fig, (ax1, ax2) = plt.subplots(1, 2, figsize=(14, 6))

            # Chart 1: Pie chart
            ax1.pie(df['CompletedRepairs'], labels=df['Name'], autopct='%1.1f%%',
                   colors=plt.cm.Set3(range(len(df))), startangle=90)
            ax1.set_title('Completed Repairs by Technician', fontsize=14, fontweight='bold')

            # Chart 2: Bar chart - Completed vs Total
            x = range(len(df))
            width = 0.35
            ax2.bar([i - width/2 for i in x], df['TotalRepairs'], width, 
                   label='Total Repairs', color=self.colors['primary'], alpha=0.8)
            ax2.bar([i + width/2 for i in x], df['CompletedRepairs'], width, 
                   label='Completed', color=self.colors['completed'], alpha=0.8)

            ax2.set_xticks(x)
            ax2.set_xticklabels(df['Name'], rotation=45, ha='right')
            ax2.set_ylabel('Count', fontsize=12, fontweight='bold')
            ax2.set_title('Technician Workload', fontsize=14, fontweight='bold')
            ax2.legend()
            ax2.grid(True, alpha=0.3, axis='y')

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
    # 4. COST ANALYSIS
    # ═══════════════════════════════════════════════════════════════════════
    def generate_cost_analysis(self):
        """Generate cost metrics by status"""
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

            # Chart 1: Total cost by status
            colors_map = [self.colors.get(s.lower(), '#999999') for s in df['Status']]
            ax1.bar(df['Status'], df['TotalCost'], color=colors_map, alpha=0.8)
            ax1.set_ylabel('Total Cost ($)', fontsize=12, fontweight='bold')
            ax1.set_title('Total Revenue by Repair Status', fontsize=14, fontweight='bold')
            ax1.grid(True, alpha=0.3, axis='y')

            # Add value labels
            for i, v in enumerate(df['TotalCost']):
                ax1.text(i, v + 50, f'${v:.2f}', ha='center', va='bottom', fontweight='bold')

            # Chart 2: Average cost by status
            ax2.bar(df['Status'], df['AvgCost'], color=colors_map, alpha=0.8)
            ax2.set_ylabel('Average Cost ($)', fontsize=12, fontweight='bold')
            ax2.set_title('Average Cost by Repair Status', fontsize=14, fontweight='bold')
            ax2.grid(True, alpha=0.3, axis='y')

            # Add value labels
            for i, v in enumerate(df['AvgCost']):
                ax2.text(i, v + 5, f'${v:.2f}', ha='center', va='bottom', fontweight='bold')

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
    # 5. TREND ANALYSIS (WEEKLY/MONTHLY)
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

            # Pivot data for stacked area chart
            pivot_df = df.pivot_table(index='Week', columns='Status', values='Count', fill_value=0)

            fig, ax = plt.subplots(figsize=(14, 6))

            status_colors = [self.colors.get(s.lower(), '#999999') for s in pivot_df.columns]
            pivot_df.plot(kind='area', stacked=True, ax=ax, color=status_colors, alpha=0.7)

            ax.set_xlabel('Week', fontsize=12, fontweight='bold')
            ax.set_ylabel('Number of Repairs', fontsize=12, fontweight='bold')
            ax.set_title('Repair Trends (Last 12 Weeks)', fontsize=16, fontweight='bold', pad=20)
            ax.legend(loc='upper left', framealpha=0.9)
            ax.grid(True, alpha=0.3)
            plt.xticks(rotation=45)

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
        """Generate all analytics reports"""
        print("\n" + "="*60)
        print("REPAIR TRACKER — ANALYTICS REPORT GENERATION")
        print("="*60 + "\n")

        results = {
            'repairs_per_day': self.generate_repairs_per_day(),
            'issue_frequency': self.generate_issue_frequency(),
            'technician_performance': self.generate_technician_performance(),
            'cost_analysis': self.generate_cost_analysis(),
            'trend_analysis': self.generate_trend_analysis()
        }

        print("\n" + "="*60)
        print(f"All reports generated in: {self.output_dir}")
        print("="*60 + "\n")

        return results


if __name__ == "__main__":
    analytics = AnalyticsService()
    analytics.generate_all_reports()