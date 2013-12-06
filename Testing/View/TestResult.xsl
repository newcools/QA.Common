<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <html lang="en-US">
      <head>
        <title>Automation Test Results</title>
        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7/jquery.js"></script>
        <script src="http://malsup.github.com/jquery.form.js"></script>
        <script type="text/javascript" src="Scripts\scriptLib.js"></script>

        <link rel="stylesheet" type="text/css" href="View\TestResultStyleA.css" />
      </head>
      <body>
        <h2 class="tutheader">Test Results</h2>
        <br/>
        <xsl:for-each select="TestResult/TestProject">
          <xsl:choose>
            <xsl:when test="@status='PartiallyPass'">
              <h3 class="partialPass">
                Project: <xsl:value-of select="@name"/> [
                Status: <xsl:value-of select="@status"/> |
                StartTime: <xsl:value-of select="@startTime"/> |
                EndTime: <xsl:value-of select="@endTime"/> ]
              </h3>
            </xsl:when>
            <xsl:when test="@status='Fail'">
              <h3 class="Fail">
                Project: <xsl:value-of select="@name"/> [
                Status: <xsl:value-of select="@status"/> |
                StartTime: <xsl:value-of select="@startTime"/> |
                EndTime: <xsl:value-of select="@endTime"/> ]
              </h3>
            </xsl:when>
            <xsl:otherwise>
              <h3 class="Pass">
                Project: <xsl:value-of select="@name"/> [
                Status: <xsl:value-of select="@status"/> |
                StartTime: <xsl:value-of select="@startTime"/> |
                EndTime: <xsl:value-of select="@endTime"/> ]
              </h3>
            </xsl:otherwise>
          </xsl:choose>
          <table class="reference">
            <tr>
              <th>Test Class</th>
              <th>Test Case</th>
              <th width="50%">Scenario</th>
              <th>Status</th>
              <th>StartTime</th>
              <th>EndTime</th>
            </tr>
            <xsl:for-each select="./TestClass">
              <xsl:variable name="testClassName" select="@name"/>
              <tr>
                <th class="testClass">
                  <xsl:value-of select="@name"/>
                </th>
                <xsl:choose>
                  <xsl:when test="@status='PartiallyPass'">
                    <td class="partialPass"/>
                    <!-- Test case column-->
                    <td class="partialPass"/>
                    <!-- Test scenario column-->
                    <td class="partialPass">Pass(p)</td>
                  </xsl:when>
                  <xsl:when test="@status='Fail'">
                    <td class="Fail"/>
                    <!-- Test case column-->
                    <td class="Fail"/>
                    <!-- Test scenario column-->
                    <td class="Fail">
                      <xsl:value-of select="@status"/>
                    </td>
                  </xsl:when>
                  <xsl:otherwise>
                    <td class="Pass"/>
                    <!-- Test case column-->
                    <td class="Pass"/>
                    <!-- Test scenario column-->
                    <td class="Pass">
                      <xsl:value-of select="@status"/>
                    </td>
                  </xsl:otherwise>
                </xsl:choose>
                <td>
                  <xsl:value-of select="@startTime"/>
                </td>
                <td>
                  <xsl:value-of select="@endTime"/>
                </td>
              </tr>
              <xsl:for-each select="./TestCase">
                <xsl:variable name="testcaseName" select="@name"/>
                <xsl:if test="count(descendant::Scenario) &gt; 0">
                  <tr>
                    <th class="testClassChildren"/>
                    <!--Test Class column-->
                    <th class="testCase">
                      <xsl:value-of select="@name"/>
                    </th>
                    <xsl:choose>
                      <xsl:when test="@status='PartiallyPass'">
                        <td class="partialPass"/>
                        <!-- Test scenario column-->
                        <td class="partialPass">Pass(p)</td>
                      </xsl:when>
                      <xsl:when test="@status='Fail'">
                        <td class="Fail"/>
                        <!-- Test scenario column-->
                        <td class="Fail">
                          <xsl:value-of select="@status"/>
                        </td>
                      </xsl:when>
                      <xsl:otherwise>
                        <td class="Pass"/>
                        <!-- Test scenario column-->
                        <td class="Pass">
                          <xsl:value-of select="@status"/>
                        </td>
                      </xsl:otherwise>
                    </xsl:choose>
                    <td>
                      <xsl:value-of select="@startTime"/>
                    </td>
                    <td>
                      <xsl:value-of select="@endTime"/>
                    </td>
                  </tr>
                  <xsl:for-each select="./Scenario">
                    <tr>
                      <th class="testClassChildren"/>
                      <!--Test Class column-->
                      <th class="testCaseChildren"/>
                      <!-- Test Case column-->
                      <td>
                        <span class="left_h2">
                          <xsl:value-of select="@name"/>
                        </span>
                        <xsl:if test="count(descendant::Message) &gt; 0">
                          <a href="#" class="left_h2 my-collapse" data-target-id="{$testClassName}-{$testcaseName}-{position()}" style="cursor:hand">
                            Errors or Messages:(<xsl:value-of select="count(descendant::Message)"/>)
                          </a>
                          <br/>
                          <div class="example" id="{$testClassName}-{$testcaseName}-{position()}">
                            <table width="100%">
                              <xsl:for-each select="./Message">
                                <tr width="100%">
                                  <td class="message">
                                    <span class="left_h3">
                                      [<xsl:value-of select="@time"/>]
                                    </span>
                                    <xsl:value-of select="current()"/>
                                  </td>
                                </tr>
                              </xsl:for-each>
                            </table>
                          </div>
                        </xsl:if>
                      </td>
                      <xsl:choose>
                        <xsl:when test="@status='PartiallyPass'">
                          <td class="partialPass">
                            <xsl:value-of select="@status"/>
                          </td>
                        </xsl:when>
                        <xsl:when test="@status='Fail'">
                          <td class="Fail">
                            <xsl:value-of select="@status"/>
                          </td>
                        </xsl:when>
                        <xsl:otherwise>
                          <td class="Pass">
                            <xsl:value-of select="@status"/>
                          </td>
                        </xsl:otherwise>
                      </xsl:choose>
                      <td>
                        <xsl:value-of select="@startTime"/>
                      </td>
                      <td>
                        <xsl:value-of select="@endTime"/>
                      </td>
                    </tr>
                  </xsl:for-each>
                </xsl:if>
                <xsl:if test="count(descendant::Scenario) = 0">
                  <tr>
                    <th class="testClassChildren"/>
                    <!--Test Class column-->
                    <th class="testCase">
                      <xsl:value-of select="@name"/>
                    </th>
                    <td>
                      <xsl:if test="count(descendant::Message) &gt; 0">
                        <a href="#" class="left_h2 my-collapse" data-target-id="{$testClassName}-{$testcaseName}-{position()}" style="cursor:hand">
                          Errors or Messages:(<xsl:value-of select="count(descendant::Message)"/>)
                        </a>
                        <br/>
                        <div class="example" id="{$testClassName}-{$testcaseName}-{position()}">
                          <table width="100%">
                            <xsl:for-each select="./Message">
                              <tr width="100%">
                                <td class="message">
                                  <span class="left_h3">
                                    [<xsl:value-of select="@time"/>]
                                  </span>
                                  <xsl:value-of select="current()"/>
                                </td>
                              </tr>
                            </xsl:for-each>
                          </table>
                        </div>
                      </xsl:if>
                    </td>
                    <xsl:choose>
                      <xsl:when test="@status='PartiallyPass'">
                        <td class="partialPass">
                          <xsl:value-of select="@status"/>
                        </td>
                      </xsl:when>
                      <xsl:when test="@status='Fail'">
                        <td class="Fail">
                          <xsl:value-of select="@status"/>
                        </td>
                      </xsl:when>
                      <xsl:otherwise>
                        <td class="Pass">
                          <xsl:value-of select="@status"/>
                        </td>
                      </xsl:otherwise>
                    </xsl:choose>
                    <td>
                      <xsl:value-of select="@startTime"/>
                    </td>
                    <td>
                      <xsl:value-of select="@endTime"/>
                    </td>
                  </tr>
                </xsl:if>
              </xsl:for-each>
            </xsl:for-each>
          </table>
          <br/>
        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>