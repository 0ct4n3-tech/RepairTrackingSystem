#!/usr/bin/env python3
"""
REPAIR TRACKER — ANALYTICS CLI
Command-line interface for generating analytics reports
"""

import sys
import argparse
from pathlib import Path
from analytics import AnalyticsService


def main():
    parser = argparse.ArgumentParser(
        description='Repair Tracker Analytics Report Generator',
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
  python main.py --all                    Generate all reports
  python main.py --repairs-per-day 60     Repairs per day (last 60 days)
  python main.py --issue-frequency 15     Top 15 most common issues
  python main.py --technician-perf        Technician performance metrics
  python main.py --cost-analysis          Cost analysis by status
  python main.py --trends                 Weekly trend analysis
        """
    )

    parser.add_argument('--all', action='store_true', 
                       help='Generate all available reports')
    parser.add_argument('--repairs-per-day', type=int, metavar='DAYS',
                       help='Generate repairs per day chart (specify days)')
    parser.add_argument('--issue-frequency', type=int, metavar='TOP_N',
                       help='Generate issue frequency chart (specify top N)')
    parser.add_argument('--technician-perf', action='store_true',
                       help='Generate technician performance report')
    parser.add_argument('--cost-analysis', action='store_true',
                       help='Generate cost analysis report')
    parser.add_argument('--trends', action='store_true',
                       help='Generate trend analysis (weekly)')
    parser.add_argument('--db', default='repairtracker.db',
                       help='Database file path (default: repairtracker.db)')
    parser.add_argument('--output', default=None,
                       help='Output directory for graphs (auto-detected if not provided)')

    args = parser.parse_args()

    if len(sys.argv) == 1:
        parser.print_help()
        return

    # Initialize analytics with explicit paths
    analytics = AnalyticsService(db_path=args.db, output_dir=args.output)

    if args.all:
        analytics.generate_all_reports()
    else:
        if args.repairs_per_day:
            analytics.generate_repairs_per_day(days=args.repairs_per_day)
        if args.issue_frequency:
            analytics.generate_issue_frequency(top_n=args.issue_frequency)
        if args.technician_perf:
            analytics.generate_technician_performance()
        if args.cost_analysis:
            analytics.generate_cost_analysis()
        if args.trends:
            analytics.generate_trend_analysis()


if __name__ == "__main__":
    main()